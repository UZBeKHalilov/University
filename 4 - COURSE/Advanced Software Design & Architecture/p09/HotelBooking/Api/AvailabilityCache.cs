using StackExchange.Redis;

namespace HotelBooking.Api;

// The three Redis commands the cache uses: GET, SET with a time to live, and INCR.
public interface IKeyValueStore
{
    string? Get(string key);
    void Set(string key, string value, TimeSpan ttl);
    void Increment(string key);
}

// The three commands, sent to Redis.
public class RedisKeyValueStore(IDatabase redis) : IKeyValueStore
{
    public string? Get(string key) => (string?)redis.StringGet(key);
    public void Set(string key, string value, TimeSpan ttl) => redis.StringSet(key, value, ttl);
    public void Increment(string key) => redis.StringIncrement(key);
}

// The cache for the free-room search (P08): cache-aside in Redis, with a TTL and a version.
// PostgreSQL is the source of truth. Redis keeps copies of search answers for 30 seconds:
// the free room numbers for one stay, such as "R7,R8". Booking and cancelling never read the cache.
public class AvailabilityCache(IKeyValueStore store)
{
    public static readonly TimeSpan Ttl = TimeSpan.FromSeconds(30);
    public const string VersionKey = "availability:version";

    // The version is part of the key, so raising it makes every older copy unreachable.
    public string Key(string checkIn, string checkOut) => $"availability:v{store.Get(VersionKey) ?? "0"}:{checkIn}:{checkOut}";

    public string? Get(string key) => store.Get(key);

    public void Set(string key, string roomNumbers) => store.Set(key, roomNumbers, Ttl);

    // A booking or a cancellation changed what is free: every cached search is now out of date.
    public void Invalidate() => store.Increment(VersionKey);
}
