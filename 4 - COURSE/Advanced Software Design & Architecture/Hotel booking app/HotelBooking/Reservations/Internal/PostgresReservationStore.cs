using HotelBooking.Rooms;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HotelBooking.Reservations.Internal;

// One row of the reservations table in db/init.sql.
public class ReservationRow
{
    public required string Id { get; set; }
    public required string GuestName { get; set; }
    public required string GuestEmail { get; set; }
    public required string Room { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public required string Status { get; set; }
}

// The tables the Reservations module owns.
public class ReservationsDb(DbContextOptions<ReservationsDb> options) : DbContext(options)
{
    public DbSet<ReservationRow> Reservations => Set<ReservationRow>();

    protected override void OnModelCreating(ModelBuilder model) => model.Entity<ReservationRow>(table =>
    {
        table.ToTable("reservations");
        table.Property(row => row.Id).HasColumnName("id");
        table.Property(row => row.GuestName).HasColumnName("guest_name");
        table.Property(row => row.GuestEmail).HasColumnName("guest_email");
        table.Property(row => row.Room).HasColumnName("room");
        table.Property(row => row.CheckIn).HasColumnName("check_in");
        table.Property(row => row.CheckOut).HasColumnName("check_out");
        table.Property(row => row.Status).HasColumnName("status");
    });
}

// Keeps reservations in PostgreSQL. Program.cs creates one per request (a unit of work): it remembers
// the reservations it hands out, and SaveChanges writes their changes.
public class PostgresReservationStore(ReservationsDb db, IReadOnlyList<Room> rooms) : IReservationStore
{
    // The name of the exclusion constraint in db/init.sql, and PostgreSQL's SQLSTATE for "a row
    // conflicted with an exclusion constraint". The store recognises exactly this one violation.
    private const string NoOverlapConstraint = "reservations_no_overlap";
    private const string ExclusionViolation = "23P01";

    private readonly List<(ReservationRow Row, Reservation Reservation)> loaded = [];

    public int Count => db.Reservations.Count();

    public bool HasOverlap(Room room, DateRange stay) => Confirmed(room, stay).Any();

    // nextval never hands the same number to two callers, and it does not wait for, or roll back
    // with, the transaction that asked for it. That is exactly what an id needs.
    // EF Core's SqlQuery<T> reads a scalar from a column it expects to be called "Value".
    public string NextReservationId() =>
        $"R{db.Database.SqlQuery<long>($"SELECT nextval('reservation_ids') AS \"Value\"").Single()}";

    public void Save(Reservation reservation)
    {
        var row = new ReservationRow
        {
            Id = reservation.Id, GuestName = reservation.Guest.Name, GuestEmail = reservation.Guest.Email,
            Room = reservation.Room.Number, CheckIn = reservation.Stay.CheckIn, CheckOut = reservation.Stay.CheckOut,
            Status = reservation.Status.ToString(),
        };
        var entry = db.Reservations.Add(row);
        try
        {
            db.SaveChanges(); // INSERT now: the constraint answers here, not at the end of the request
        }
        catch (DbUpdateException error) when (IsOverlapRefusal(error))
        {
            // EF Core still has this row tracked as Added, so the SaveChanges at the end of the
            // request would try the same INSERT again. Detaching it is this language's version of
            // rolling back to a savepoint: the refused statement is forgotten, the request is not.
            entry.State = EntityState.Detached;
            throw new OverlapRejectedException($"{reservation.Room.Number} already has a confirmed reservation for these nights.");
        }
    }

    // A duplicate id, or anything else, is a real fault and must not be read as a double booking.
    private static bool IsOverlapRefusal(DbUpdateException error) =>
        error.InnerException is PostgresException { SqlState: ExclusionViolation } postgres
        && postgres.ConstraintName == NoOverlapConstraint;

    public Reservation? Find(string id)
    {
        var row = db.Reservations.Find(id);
        if (row is null) return null;
        var reservation = new Reservation(row.Id, new Guest(row.GuestName, row.GuestEmail),
            rooms.First(room => room.Number == row.Room), new DateRange(row.CheckIn, row.CheckOut));
        if (row.Status == "Cancelled") reservation.Cancel();
        loaded.Add((row, reservation));
        return reservation;
    }

    public int CountConfirmed(Room room, DateRange stay) => Confirmed(room, stay).Count();

    // The end of the request: copy each loaded reservation's status to its row; EF Core writes what changed.
    public void SaveChanges()
    {
        foreach (var (row, reservation) in loaded) row.Status = reservation.Status.ToString();
        db.SaveChanges();
    }

    // Confirmed reservations of this room that share at least one night with the stay.
    private IQueryable<ReservationRow> Confirmed(Room room, DateRange stay) =>
        db.Reservations.Where(row => row.Room == room.Number && row.Status == "Confirmed"
            && row.CheckIn < stay.CheckOut && stay.CheckIn < row.CheckOut);
}
