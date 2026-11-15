using HotelBooking.Rooms;

namespace HotelBooking.Reservations.Internal;

// Keeps reservations in a list. P07 adds a PostgreSQL store with the same interface.
public class InMemoryReservationStore : IReservationStore
{
    private readonly List<Reservation> _rows = [];

    // ASP.NET Core answers several requests at once on different threads, and this list is the
    // whole database here. Checking and adding under one lock is this store's version of the
    // exclusion constraint.
    private readonly Lock _gate = new();

    public int Count
    {
        get { lock (_gate) return _rows.Count; }
    }

    public bool HasOverlap(Room room, DateRange stay)
    {
        lock (_gate) return _rows.Any(row => row.Blocks(room, stay));
    }

    // Safe here for the same reason Save is: it holds the same lock.
    public string NextReservationId()
    {
        lock (_gate) return $"R{101 + _rows.Count}";
    }

    public void Save(Reservation reservation)
    {
        lock (_gate)
        {
            if (_rows.Any(row => row.Blocks(reservation.Room, reservation.Stay)))
                throw new OverlapRejectedException($"{reservation.Room.Number} already has a confirmed reservation for these nights.");
            _rows.Add(reservation);
        }
    }

    public Reservation? Find(string id)
    {
        lock (_gate) return _rows.FirstOrDefault(row => row.Id == id);
    }

    public int CountConfirmed(Room room, DateRange stay)
    {
        lock (_gate) return _rows.Count(row => row.Blocks(room, stay));
    }
}
