namespace HotelBooking.Reservations;

// The stay: check-in day included, check-out day excluded (half-open).
public readonly record struct DateRange(DateOnly CheckIn, DateOnly CheckOut)
{
    public bool IsValid() => CheckOut > CheckIn;

    // Two stays overlap when they share at least one night.
    public bool Overlaps(DateRange other) =>
        CheckIn < other.CheckOut && other.CheckIn < CheckOut;
}
