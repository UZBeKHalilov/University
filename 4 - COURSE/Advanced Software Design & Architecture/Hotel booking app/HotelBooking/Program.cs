// P08 · Hotel booking: the P07 API with a Redis cache for the free-room search.
// dotnet run --project HotelBooking            -> the API on http://localhost:5080 (first: docker compose up -d)
// dotnet run --project HotelBooking -- --demo  -> the P06 demo, with the data in lists
// dotnet run --project HotelBooking -- --race  -> two guests book the same room at the same moment
using HotelBooking.Api;
using HotelBooking.Notifications.Internal;
using HotelBooking.Reservations;
using HotelBooking.Reservations.Internal;
using HotelBooking.Rooms;
using HotelBooking.Rooms.Internal;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var r7 = new Room("R7", "Standard");
var r8 = new Room("R8", "Deluxe");
var maya = new Guest("Maya", "maya@example.com");
var omar = new Guest("Omar", "omar@example.com");
var nora = new Guest("Nora", "nora@example.com");

// The wiring: the only app code that names the Internal classes of the modules.
var catalog = new InMemoryRoomCatalog([r7, r8]);

if (args.Contains("--race")) RunRace();
else if (args.Contains("--demo")) RunDemo();
else RunApi();

void RunApi()
{
    // The wiring for the web app: PostgreSQL and Redis from docker-compose.yml
    // (HOTEL_DB_PORT and HOTEL_REDIS_PORT only if 5432 or 6379 is taken).
    var builder = WebApplication.CreateBuilder(args);
    var connection = $"Host=localhost;Port={Environment.GetEnvironmentVariable("HOTEL_DB_PORT") ?? "5432"};Database=hotel;Username=hotel;Password=hotel";
    builder.Services.AddDbContextFactory<RoomsDb>(options => options.UseNpgsql(connection));
    builder.Services.AddDbContextFactory<ReservationsDb>(options => options.UseNpgsql(connection));
    var app = builder.Build();

    var rooms = new PostgresRoomCatalog(app.Services.GetRequiredService<IDbContextFactory<RoomsDb>>()).All();
    var databases = app.Services.GetRequiredService<IDbContextFactory<ReservationsDb>>();
    var email = new EmailNotificationSender();
    var stats = new Stats();
    // Connect stops here if Redis is not running.
    var redis = ConnectionMultiplexer.Connect($"localhost:{Environment.GetEnvironmentVariable("HOTEL_REDIS_PORT") ?? "6379"}");
    var cache = new AvailabilityCache(new RedisKeyValueStore(redis.GetDatabase()));
    // Step 1 widens the gap between the check and the save, so the race happens on every run.
    var slowStepMs = int.Parse(Environment.GetEnvironmentVariable("HOTEL_SLOW_STEP") ?? "0") * 1000;

    // One request: a fresh store over its own DbContext, counted for GET /stats; save what changed at the end.
    IResult UnitOfWork(Func<BookingService, IResult> work)
    {
        using var db = databases.CreateDbContext();
        var store = new PostgresReservationStore(db, rooms);
        var result = work(new BookingService(rooms, new CountingReservationStore(store, stats), email, slowStepMs));
        store.SaveChanges();
        return result;
    }

    HotelApi.Map(app, rooms, UnitOfWork, stats, cache);
    // Step 4 starts a second instance with HOTEL_PORT=5081, against the same database.
    app.Run($"http://localhost:{Environment.GetEnvironmentVariable("HOTEL_PORT") ?? "5080"}");
}

void RunDemo()
{
    var store = new InMemoryReservationStore();
    var email = new EmailNotificationSender();
    var service = new BookingService(catalog.All(), store, email);

    Console.WriteLine($"Free rooms 12-14 Nov: {Names(service.FindFreeRooms(Nov(12, 14)))}");
    Console.WriteLine($"Maya books R7 12-14 Nov: {service.Reserve(maya, r7, Nov(12, 14))}");
    Console.WriteLine($"Omar books R7 13-15 Nov: {service.Reserve(omar, r7, Nov(13, 15))}");
    Console.WriteLine($"Nora books R7 14-16 Nov: {service.Reserve(nora, r7, Nov(14, 16))}");
    Console.WriteLine($"Omar books R8 15-15 Nov: {service.Reserve(omar, r8, Nov(15, 15))}");
    Console.WriteLine($"Free rooms 12-14 Nov: {Names(service.FindFreeRooms(Nov(12, 14)))}");
    Console.WriteLine($"Maya cancels R101: {service.Cancel("R101")}");
    Console.WriteLine($"Free rooms 12-14 Nov: {Names(service.FindFreeRooms(Nov(12, 14)))}");
    Console.WriteLine($"Confirmation emails sent: {email.Sent}");
}

void RunRace()
{
    var store = new InMemoryReservationStore();
    // Each request now spends half a second between checking and saving.
    var service = new BookingService(catalog.All(), store, new EmailNotificationSender(), slowStepMs: 500);
    var stay = Nov(12, 14);
    BookingResult? mayaResult = null, omarResult = null;

    Console.WriteLine("Maya and Omar both book R7 for 12-14 Nov at the same moment...");
    var mayaRequest = new Thread(() => mayaResult = service.Reserve(maya, r7, stay));
    var omarRequest = new Thread(() => omarResult = service.Reserve(omar, r7, stay));
    mayaRequest.Start();
    Thread.Sleep(100); // Omar clicks a moment after Maya
    omarRequest.Start();
    mayaRequest.Join();
    omarRequest.Join();

    Console.WriteLine($"Maya: {mayaResult}");
    Console.WriteLine($"Omar: {omarResult}");
    var confirmed = store.CountConfirmed(r7, stay);
    Console.WriteLine($"Confirmed reservations for R7 on 12-14 Nov: {confirmed}");
    Console.WriteLine(confirmed > 1
        ? "Double booking! Both requests checked before either one saved."
        : "Only one guest has R7.");
}

DateRange Nov(int checkIn, int checkOut) =>
    new(new DateOnly(2026, 11, checkIn), new DateOnly(2026, 11, checkOut));

string Names(IEnumerable<Room> rooms) => string.Join(", ", rooms.Select(room => room.Number));
