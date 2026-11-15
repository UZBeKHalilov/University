using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using HotelBooking.Reservations;
using HotelBooking.Rooms;

namespace HotelBooking.Api;

// Runs one request's work with a BookingService, then saves whatever changed.
public delegate IResult UnitOfWork(Func<BookingService, IResult> work);

// The HTTP API: three endpoints that turn HTTP requests into BookingService calls and back, GET /stats
// with what the searches cost, and the cache for the search. It lives outside the four modules and uses
// only their public contracts; Program.cs decides which store the endpoints use, through the unit of work.
public static class HotelApi
{
    private static readonly string[] Fields = ["guestName", "guestEmail", "room", "checkIn", "checkOut"];
    private const string BodyDetail = "The body must be JSON with guestName, guestEmail, room, checkIn and checkOut.";
    private const string DateDetail = "checkIn and checkOut must be dates such as 2026-11-12.";

    // Problem details (RFC 9457): every error has the same three keys.
    private static IResult Problem(int status, string title, string detail) =>
        Results.Json(new { title, status, detail }, contentType: "application/problem+json", statusCode: status);

    // A date written exactly like 2026-11-12, otherwise null.
    private static DateOnly? ParseDate(string? text) =>
        DateOnly.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var day) ? day : null;

    // Turns a booking answer into an HTTP status code and a JSON body (L04).
    private static IResult ToHttp(BookingResult result) => result.Outcome switch
    {
        "Accepted" => Results.Json(new { id = result.ReservationId, status = "Confirmed", notificationFailed = result.NotificationFailed }, statusCode: 201),
        "Unavailable" => Problem(409, "Unavailable", "The room is booked for at least one night of this stay."),
        _ => Problem(400, "InvalidDates", "checkOut must be after checkIn."),
    };

    public static void Map(WebApplication app, IReadOnlyList<Room> rooms, UnitOfWork unitOfWork, Stats stats, AvailabilityCache cache)
    {
        app.MapGet("/openapi.yaml", () => Results.Text(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "openapi.yaml")), "application/yaml"));

        app.MapGet("/stats", () => Results.Json(stats.Answer()));

        app.MapGet("/rooms/availability", (string? checkIn, string? checkOut, string? limit, string? after) =>
        {
            if (ParseDate(checkIn) is not { } start || ParseDate(checkOut) is not { } end) return Problem(400, "InvalidDates", DateDetail);
            var stay = new DateRange(start, end);
            if (!stay.IsValid()) return Problem(400, "InvalidDates", "checkOut must be after checkIn.");
            limit ??= "20";
            if (!Regex.IsMatch(limit, @"^[0-9]{1,3}\z") || int.Parse(limit) is < 1 or > 100)
                return Problem(400, "InvalidRequest", "limit must be a whole number from 1 to 100.");

            stats.AddSearch();
            // Cache-aside (L04): Redis first; only on a miss ask PostgreSQL, then keep a copy.
            var key = cache.Key(checkIn!, checkOut!);
            if (cache.Get(key) is { } cached)
            {
                stats.AddCacheHit();
                var numbers = cached.Split(',');
                return Page(rooms.Where(room => numbers.Contains(room.Number)));
            }
            stats.AddCacheMiss();
            return unitOfWork(service =>
            {
                var free = service.FindFreeRooms(stay);
                cache.Set(key, string.Join(",", free.Select(room => room.Number).Order(StringComparer.Ordinal)));
                return Page(free);
            });

            IResult Page(IEnumerable<Room> free)
            {
                // Cursor pagination (L04): the rooms after the last one the client has seen, in text order.
                var rest = free.Where(room => string.CompareOrdinal(room.Number, after ?? "") > 0)
                    .OrderBy(room => room.Number, StringComparer.Ordinal).ToList();
                var page = rest.Take(int.Parse(limit)).ToList();
                return Results.Json(new
                {
                    checkIn, checkOut,
                    rooms = page.Select(room => new { number = room.Number, type = room.Type }),
                    next = rest.Count > page.Count ? page[^1].Number : null,
                });
            }
        });

        app.MapPost("/reservations", async (HttpRequest request) =>
        {
            if (!request.HasJsonContentType()) return Problem(400, "InvalidRequest", BodyDetail);
            JsonElement body;
            try
            {
                body = (await JsonDocument.ParseAsync(request.Body)).RootElement;
            }
            catch (JsonException)
            {
                return Problem(400, "InvalidRequest", BodyDetail);
            }
            string? Text(string field) => body.ValueKind == JsonValueKind.Object && body.TryGetProperty(field, out var value)
                && value.ValueKind == JsonValueKind.String && value.GetString() is { Length: > 0 } text ? text : null;
            if (Fields.Any(field => Text(field) is null)) return Problem(400, "InvalidRequest", BodyDetail);
            if (ParseDate(Text("checkIn")) is not { } start || ParseDate(Text("checkOut")) is not { } end) return Problem(400, "InvalidDates", DateDetail);
            if (rooms.FirstOrDefault(room => room.Number == Text("room")) is not { } room) return Problem(400, "UnknownRoom", $"There is no room {Text("room")}.");

            var guest = new Guest(Text("guestName")!, Text("guestEmail")!);
            var answer = unitOfWork(service => ToHttp(service.Reserve(guest, room, new DateRange(start, end))));
            if (answer is IStatusCodeHttpResult { StatusCode: 201 }) cache.Invalidate(); // after the unit of work: the booking is saved in PostgreSQL
            return answer;
        });

        app.MapDelete("/reservations/{id}", (string id) =>
        {
            var answer = unitOfWork(service => service.Cancel(id)
                ? Results.NoContent()
                : Problem(404, "NotFound", $"There is no confirmed reservation {id}."));
            if (answer is IStatusCodeHttpResult { StatusCode: 204 }) cache.Invalidate(); // after the unit of work: the cancel is saved in PostgreSQL
            return answer;
        });
    }
}
