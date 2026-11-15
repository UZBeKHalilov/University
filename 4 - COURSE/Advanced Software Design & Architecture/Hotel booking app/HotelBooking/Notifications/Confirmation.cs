namespace HotelBooking.Notifications;

// Notifications' own message: it no longer needs the Reservation class.
public record Confirmation(string ReservationId, string GuestEmail, string RoomNumber, DateOnly CheckIn, DateOnly CheckOut);
