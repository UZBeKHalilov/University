// Sends two POST /reservations for the same room and the same nights at the same moment, and prints
// what each one got back and how many confirmed reservations the room ended up with.
//
//   dotnet run tools/race-http.cs                            both requests to http://localhost:5080
//   dotnet run tools/race-http.cs http://localhost:5081      Omar's request to the second instance
//
// Start the app with HOTEL_SLOW_STEP=1 first. Without it the gap between the check and the save is
// too small to land in reliably, and the race shows up only now and then.
//
// Maya's request goes first and Omar's 500 ms later, so the two reach the gap together and leave it
// in a known order. That is what makes the output the same on every run; it is not what makes the
// bug possible. The bug is possible because checking and saving are two separate statements.
//
// A warm-up request is sent before the two bookings. The very first request to a freshly started
// app pays for connection pools, servlet initialisation and JIT, which can cost more than the
// head start Maya is given - and then Omar wins instead and the output comes out the other way
// round. The warm-up cannot reach the booking path itself, though - GET /stats never inserts - so
// Maya's request is still the first to compile the insert, which measured as much as 140 ms more
// than Omar's. Half a second of head start covers that and leaves both requests inside the gap.
//
// A file-based app (dotnet run <file>.cs) is compiled ready for ahead-of-time publishing, and that
// turns off the reflection the typed JSON helpers below need - PostAsJsonAsync would throw
// "Reflection-based serialization has been disabled for this application". This tool is run, never
// published, so the AOT settings do nothing for it and are turned off.
#:property PublishAot=false
using System.Net;
using System.Net.Http.Json;

var mayaBase = "http://localhost:5080";
var omarBase = args.Length > 0 ? args[0] : mayaBase;
var maya = new { guestName = "Maya", guestEmail = "maya@example.com", room = "R7", checkIn = "2026-11-12", checkOut = "2026-11-14" };
var omar = new { guestName = "Omar", guestEmail = "omar@example.com", room = "R7", checkIn = "2026-11-12", checkOut = "2026-11-14" };

using var client = new HttpClient();
await client.GetStringAsync($"{mayaBase}/stats"); // the warm-up: see the note at the top

Console.WriteLine("Two bookings for R7, 2026-11-12 to 2026-11-14, sent at the same moment.");
if (omarBase != mayaBase) Console.WriteLine($"Maya -> {mayaBase}   Omar -> {omarBase}");
Console.WriteLine();

// Both requests are started before either is awaited: that is what "at the same moment" means here.
var mayaRequest = Book(mayaBase, maya);
var omarRequest = Delayed(500, () => Book(omarBase, omar)); // Omar clicks a moment after Maya
var answers = await Task.WhenAll(mayaRequest, omarRequest);

Console.WriteLine($"Maya -> {answers[0]}");
Console.WriteLine($"Omar -> {answers[1]}");
Console.WriteLine();

var accepted = answers.Count(answer => answer.StartsWith("201"));
Console.WriteLine($"Confirmed reservations for R7 on 12-14 Nov: {accepted}");
Console.WriteLine(accepted > 1
    ? "DOUBLE BOOKED: the room has two confirmed reservations for the same nights."
    : "CORRECT: one booking was accepted and one was refused.");

async Task<string> Book(string baseAddress, object body)
{
    var response = await client.PostAsJsonAsync($"{baseAddress}/reservations", body);
    var reason = response.StatusCode switch
    {
        HttpStatusCode.Created => "201 Created   ",
        HttpStatusCode.Conflict => "409 Conflict  ",
        HttpStatusCode.BadRequest => "400 Bad Request  ",
        var other => $"{(int)other} {other}  ",
    };
    return reason + await response.Content.ReadAsStringAsync();
}

async Task<string> Delayed(int milliseconds, Func<Task<string>> work)
{
    await Task.Delay(milliseconds);
    return await work();
}
