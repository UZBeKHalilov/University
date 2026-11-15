namespace HotelBooking.Reservations;

public record BookingResult(string Outcome, string? ReservationId = null, bool NotificationFailed = false)
{
    public override string ToString() => ReservationId is null ? Outcome : $"{Outcome} {ReservationId}";
}
