using HotelBooking.Rooms;

namespace HotelBooking.Reservations;

// What BookingService needs from storage. Today a list in memory; from P07 a database.
public interface IReservationStore
{
    int Count { get; }
    bool HasOverlap(Room room, DateRange stay);

    // The id for the next reservation.
    //
    // The store owns this, because only the store can hand out an id that no other request can
    // also be given. Counting the rows and adding one cannot: two requests that count before
    // either has saved both get R101, and the second INSERT then fails on the primary key instead
    // of on the booking rule. PostgreSQL answers from a sequence; the list store counts, which is
    // safe there because it holds a lock.
    string NextReservationId();

    // Writes the reservation.
    //
    // Throws OverlapRejectedException if writing it would give the room two Confirmed reservations
    // covering the same night. The pre-check in Reserve usually catches that first; this promise is
    // what makes the rule true when two requests check at the same moment.
    void Save(Reservation reservation);

    Reservation? Find(string id);
    int CountConfirmed(Room room, DateRange stay);
}
