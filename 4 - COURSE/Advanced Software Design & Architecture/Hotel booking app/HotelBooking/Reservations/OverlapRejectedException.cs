namespace HotelBooking.Reservations;

// The store refused a save because it would give the room two Confirmed reservations covering the
// same night.
//
// Every store promises this, and the promise is part of IReservationStore. How a store keeps it is
// its own business: the list store checks inside Save, and PostgreSQL refuses the INSERT with the
// exclusion constraint reservations_no_overlap. BookingService turns it into Unavailable - the same
// answer the pre-check gives - so a guest never sees a database error.
public class OverlapRejectedException(string message) : Exception(message);
