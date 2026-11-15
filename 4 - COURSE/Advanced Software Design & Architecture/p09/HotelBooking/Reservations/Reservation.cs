using HotelBooking.Rooms;

namespace HotelBooking.Reservations;

public class Reservation
{
    public Reservation(string id, Guest guest, Room room, DateRange stay)
    {
        Id = id;
        Guest = guest;
        Room = room;
        Stay = stay;
    }

    public string Id { get; }
    public Guest Guest { get; }
    public Room Room { get; }
    public DateRange Stay { get; }
    public ReservationStatus Status { get; private set; } = ReservationStatus.Confirmed;

    // A reservation blocks a request when it is still Confirmed for the same room and shares a night.
    public bool Blocks(Room room, DateRange stay) =>
        Status == ReservationStatus.Confirmed && Room == room && Stay.Overlaps(stay);

    public bool Cancel()
    {
        if (Status == ReservationStatus.Cancelled) return false;
        Status = ReservationStatus.Cancelled;
        return true;
    }
}
