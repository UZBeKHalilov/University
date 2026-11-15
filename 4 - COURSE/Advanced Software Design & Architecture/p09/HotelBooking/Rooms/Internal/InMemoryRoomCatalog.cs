namespace HotelBooking.Rooms.Internal;

// Keeps the hotel's rooms in a list. Only Program.cs creates it.
public class InMemoryRoomCatalog(IReadOnlyList<Room> rooms) : IRoomCatalog
{
    public IReadOnlyList<Room> All() => rooms;
}
