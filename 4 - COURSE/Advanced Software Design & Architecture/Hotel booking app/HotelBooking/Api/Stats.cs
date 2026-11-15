using HotelBooking.Reservations;
using HotelBooking.Rooms;

namespace HotelBooking.Api;

// The four counters of GET /stats. ASP.NET Core runs requests on several threads at once,
// so Interlocked makes every +1 exact.
public class Stats
{
    private long _searches, _databaseQueries, _cacheHits, _cacheMisses;

    public void AddSearch() => Interlocked.Increment(ref _searches);
    public void AddDatabaseQuery() => Interlocked.Increment(ref _databaseQueries);
    public void AddCacheHit() => Interlocked.Increment(ref _cacheHits);
    public void AddCacheMiss() => Interlocked.Increment(ref _cacheMisses);

    public object Answer() => new
    {
        searches = Interlocked.Read(ref _searches), databaseQueries = Interlocked.Read(ref _databaseQueries),
        cacheHits = Interlocked.Read(ref _cacheHits), cacheMisses = Interlocked.Read(ref _cacheMisses),
    };
}

// Passes every call on to the real store and counts it: each call is one question to the database.
public class CountingReservationStore(IReservationStore store, Stats stats) : IReservationStore
{
    public int Count
    {
        get
        {
            stats.AddDatabaseQuery();
            return store.Count;
        }
    }

    public bool HasOverlap(Room room, DateRange stay)
    {
        stats.AddDatabaseQuery();
        return store.HasOverlap(room, stay);
    }

    public string NextReservationId()
    {
        stats.AddDatabaseQuery();
        return store.NextReservationId();
    }

    public void Save(Reservation reservation)
    {
        stats.AddDatabaseQuery();
        store.Save(reservation);
    }

    public Reservation? Find(string id)
    {
        stats.AddDatabaseQuery();
        return store.Find(id);
    }

    public int CountConfirmed(Room room, DateRange stay)
    {
        stats.AddDatabaseQuery();
        return store.CountConfirmed(room, stay);
    }
}
