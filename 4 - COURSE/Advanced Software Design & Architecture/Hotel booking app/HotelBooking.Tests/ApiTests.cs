// The eight API tests: HTTP in, HTTP out, with the reservations in a list and the cache in a
// dictionary. The two P09 tests are the last two: they are about what happens when two bookings for
// the same room and the same nights are made at the same moment.
//
// The list store keeps the same promise PostgreSQL's exclusion constraint keeps - Save throws
// OverlapRejectedException rather than double-booking a room - so these tests need neither Docker
// nor a database, and they still test the rule the constraint protects.
// Run them with the other tests from the hotel-booking folder:  dotnet test HotelBooking.Tests
using System.Net;
using System.Net.Http.Json;
using HotelBooking.Api;
using HotelBooking.Notifications.Internal;
using HotelBooking.Reservations;
using HotelBooking.Reservations.Internal;
using HotelBooking.Rooms;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace HotelBooking.Tests;

public class ApiTests
{
    private static readonly Room[] Rooms = [new("R7", "Standard"), new("R8", "Deluxe")];
    private const string Search = "/rooms/availability?checkIn=2026-11-12&checkOut=2026-11-14";
    private static readonly object Maya = new { guestName = "Maya", guestEmail = "maya@example.com", room = "R7", checkIn = "2026-11-12", checkOut = "2026-11-14" };
    private const string BothFree = """{"checkIn":"2026-11-12","checkOut":"2026-11-14","rooms":[{"number":"R7","type":"Standard"},{"number":"R8","type":"Deluxe"}],"next":null}""";
    private const string OnlyR8 = """{"checkIn":"2026-11-12","checkOut":"2026-11-14","rooms":[{"number":"R8","type":"Deluxe"}],"next":null}""";
    private static readonly object Omar = new { guestName = "Omar", guestEmail = "omar@example.com", room = "R7", checkIn = "2026-11-13", checkOut = "2026-11-15" };

    // Redis's three commands on a dictionary. The tests run in a second, so the 30 s TTL never ends.
    private class InMemoryKeyValueStore : IKeyValueStore
    {
        private readonly Dictionary<string, string> _values = [];
        public string? Get(string key) => _values.GetValueOrDefault(key);
        public void Set(string key, string value, TimeSpan ttl) => _values[key] = value;
        public void Increment(string key) => _values[key] = (int.Parse(_values.GetValueOrDefault(key, "0")) + 1).ToString();
    }

    // The web app with the in-memory store and cache, on ASP.NET Core's test server: the client sends requests without a network.
    private static async Task<HttpClient> NewClient() => (await NewClientWithStore()).Client;

    // slowStepMs widens the gap between the check and the save, the way HOTEL_SLOW_STEP does for
    // the real app; the store is handed back so a test can count what was actually saved.
    private static async Task<(HttpClient Client, InMemoryReservationStore Store)> NewClientWithStore(int slowStepMs = 0)
    {
        var stats = new Stats();
        var store = new InMemoryReservationStore();
        var service = new BookingService(Rooms, new CountingReservationStore(store, stats), new EmailNotificationSender(), slowStepMs);
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        var app = builder.Build();
        HotelApi.Map(app, Rooms, work => work(service), stats, new AvailabilityCache(new InMemoryKeyValueStore())); // a list in memory has nothing to save at the end of a request
        await app.StartAsync();
        return (app.GetTestClient(), store);
    }

    private static async Task<string?> Title(HttpResponseMessage response) =>
        (await response.Content.ReadFromJsonAsync<Dictionary<string, object>>())?["title"].ToString();

    [Fact]
    public async Task Search_answers_200_with_the_free_rooms()
    {
        var response = await (await NewClient()).GetAsync(Search);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("""{"checkIn":"2026-11-12","checkOut":"2026-11-14","rooms":[{"number":"R7","type":"Standard"},{"number":"R8","type":"Deluxe"}],"next":null}""",
            await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Booking_answers_201_then_409_for_a_shared_night()
    {
        var client = await NewClient();
        var first = await client.PostAsJsonAsync("/reservations", Maya);
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal("""{"id":"R101","status":"Confirmed","notificationFailed":false}""", await first.Content.ReadAsStringAsync());
        var second = await client.PostAsJsonAsync("/reservations", Omar);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        Assert.Equal("Unavailable", await Title(second));
    }

    [Fact]
    public async Task Wrong_dates_answer_400_with_problem_details()
    {
        var response = await (await NewClient()).PostAsJsonAsync("/reservations",
            new { guestName = "Maya", guestEmail = "maya@example.com", room = "R7", checkIn = "2026-11-12", checkOut = "2026-11-12" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("""{"title":"InvalidDates","status":400,"detail":"checkOut must be after checkIn."}""", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Cancel_answers_204_then_404()
    {
        var client = await NewClient();
        await client.PostAsJsonAsync("/reservations", Maya);
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync("/reservations/R101")).StatusCode);
        var second = await client.DeleteAsync("/reservations/R101");
        Assert.Equal(HttpStatusCode.NotFound, second.StatusCode);
        Assert.Equal("NotFound", await Title(second));
    }

    [Fact]
    public async Task The_second_search_is_answered_by_the_cache()
    {
        var client = await NewClient();
        var first = await client.GetStringAsync(Search);
        Assert.Equal(first, await client.GetStringAsync(Search));
        Assert.Equal("""{"searches":2,"databaseQueries":2,"cacheHits":1,"cacheMisses":1}""", await client.GetStringAsync("/stats"));
    }

    [Fact]
    public async Task A_booking_and_a_cancel_make_the_next_search_fresh()
    {
        var client = await NewClient();
        Assert.Equal(BothFree, await client.GetStringAsync(Search));
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/reservations", Maya)).StatusCode);
        Assert.Equal(OnlyR8, await client.GetStringAsync(Search));
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync("/reservations/R101")).StatusCode);
        Assert.Equal(BothFree, await client.GetStringAsync(Search));
    }

    // ---------------------------------------------------------------- P09
    [Fact]
    public async Task Two_bookings_at_the_same_moment_give_the_room_one_reservation()
    {
        // Step 1's race, as a test. Both requests pass the pre-check; only one may be saved.
        var (client, store) = await NewClientWithStore(slowStepMs: 300);
        var sameStay = new { guestName = "Omar", guestEmail = "omar@example.com", room = "R7", checkIn = "2026-11-12", checkOut = "2026-11-14" };

        // Started together, awaited together: Omar's check runs while Maya is still in the gap.
        var maya = client.PostAsJsonAsync("/reservations", Maya);
        var omar = client.PostAsJsonAsync("/reservations", sameStay);
        var answers = await Task.WhenAll(maya, omar);

        Assert.Equal([HttpStatusCode.Created, HttpStatusCode.Conflict], answers.Select(answer => answer.StatusCode).OrderBy(code => code));
        Assert.Equal(1, store.CountConfirmed(Rooms[0], new DateRange(new DateOnly(2026, 11, 12), new DateOnly(2026, 11, 14))));
    }

    [Fact]
    public void The_store_refuses_an_overlapping_save_even_without_the_pre_check()
    {
        // The promise the exclusion constraint keeps in PostgreSQL, kept by the list store too.
        // Nothing calls HasOverlap here: Save itself must refuse.
        var store = new InMemoryReservationStore();
        var stay = new DateRange(new DateOnly(2026, 11, 12), new DateOnly(2026, 11, 14));
        store.Save(new Reservation("R101", new Guest("Maya", "maya@example.com"), Rooms[0], stay));

        var overlapping = new Reservation("R102", new Guest("Omar", "omar@example.com"), Rooms[0],
            new DateRange(new DateOnly(2026, 11, 13), new DateOnly(2026, 11, 15)));
        Assert.Throws<OverlapRejectedException>(() => store.Save(overlapping));
        Assert.Equal(1, store.CountConfirmed(Rooms[0], stay));
    }
}
