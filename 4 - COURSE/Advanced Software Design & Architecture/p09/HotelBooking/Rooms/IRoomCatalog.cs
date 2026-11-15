namespace HotelBooking.Rooms;

// Rooms' public contract: which rooms the hotel sells. From P07 they come from a database.
public interface IRoomCatalog
{
    IReadOnlyList<Room> All();
}
