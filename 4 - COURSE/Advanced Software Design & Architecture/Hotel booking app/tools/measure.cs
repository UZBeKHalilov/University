// Sends 1,000 searches to the running app, one after another, and prints how long they took
// and what they cost: the difference of GET /stats before and after.
//
// dotnet run tools/measure.cs                         -> 12-14 Nov
// dotnet run tools/measure.cs 2026-12-31 2027-01-02   -> other dates
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

const int searches = 1000;
string[] names = ["searches", "databaseQueries", "cacheHits", "cacheMisses"];
var (checkIn, checkOut) = args.Length >= 2 ? (args[0], args[1]) : ("2026-11-12", "2026-11-14");
var culture = CultureInfo.InvariantCulture;

// One client: it keeps the connection open and reuses it for every request.
using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5080") };

async Task<string> Get(string path)
{
    var response = await client.GetAsync(path);
    var body = await response.Content.ReadAsStringAsync();
    if ((int)response.StatusCode != 200)
    {
        Console.Error.WriteLine($"GET {path} answered {(int)response.StatusCode}: {body}");
        Environment.Exit(1);
    }
    return body;
}

async Task<JsonElement> Stats() => JsonDocument.Parse(await Get("/stats")).RootElement;

var before = await Stats();
var times = new List<double>();
var started = Stopwatch.StartNew();
for (var i = 0; i < searches; i++)
{
    var start = Stopwatch.GetTimestamp();
    await Get($"/rooms/availability?checkIn={checkIn}&checkOut={checkOut}");
    times.Add(Stopwatch.GetElapsedTime(start).TotalMilliseconds);
}
var total = started.Elapsed.TotalSeconds;
var after = await Stats();

times.Sort();
var p95 = times[(int)Math.Ceiling(0.95 * searches) - 1]; // 950 of the 1,000 searches were this fast or faster
var difference = string.Join(",", names.Select(name => $"\"{name}\":{after.GetProperty(name).GetInt64() - before.GetProperty(name).GetInt64()}"));
Console.WriteLine($"{searches} searches for {checkIn} to {checkOut}");
Console.WriteLine($"Average: {times.Average().ToString("F1", culture)} ms");
Console.WriteLine($"p95: {p95.ToString("F1", culture)} ms");
Console.WriteLine($"Total: {total.ToString("F1", culture)} s");
Console.WriteLine($"GET /stats difference: {{{difference}}}");
