using HotelBooking.Notifications;
using HotelBooking.Rooms;

namespace HotelBooking.Reservations;

// Owns the booking rule. Uses only the public contracts of Rooms and Notifications.
public class BookingService(IReadOnlyList<Room> rooms, IReservationStore store, INotificationSender notifications, int slowStepMs = 0)
{
    public IReadOnlyList<Room> FindFreeRooms(DateRange stay) =>
        rooms.Where(room => !store.HasOverlap(room, stay)).ToList();

    public BookingResult Reserve(Guest guest, Room room, DateRange stay)
    {
        if (!stay.IsValid()) return new("InvalidDates");
        if (store.HasOverlap(room, stay)) return new("Unavailable");

        Thread.Sleep(slowStepMs); // 0 normally; the race tool makes this step slow

        var reservation = new Reservation(store.NextReservationId(), guest, room, stay);
        try
        {
            store.Save(reservation);
        }
        catch (OverlapRejectedException)
        {
            // Another request saved an overlapping reservation after our check and before our save.
            // The check was honest when it ran; it had simply stopped being true. The guest gets
            // the same answer either way.
            return new("Unavailable");
        }
        try
        {
            // Notifications gets its own message: the five fields it needs, not the Reservation.
            notifications.SendConfirmation(new Confirmation(reservation.Id, guest.Email, room.Number, stay.CheckIn, stay.CheckOut));
        }
        catch (Exception)
        {
            // R6: the booking is already saved, so a failed message must not undo or hide it.
            return new("Accepted", reservation.Id, NotificationFailed: true);
        }
        return new("Accepted", reservation.Id);
    }

    public bool Cancel(string reservationId) => store.Find(reservationId)?.Cancel() ?? false;
}
