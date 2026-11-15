using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Rooms.Internal;

// One row of the rooms table in db/init.sql.
public class RoomRow
{
    public required string Number { get; set; }
    public required string Type { get; set; }
}

// The tables the Rooms module owns.
public class RoomsDb(DbContextOptions<RoomsDb> options) : DbContext(options)
{
    public DbSet<RoomRow> Rooms => Set<RoomRow>();

    protected override void OnModelCreating(ModelBuilder model) => model.Entity<RoomRow>(table =>
    {
        table.ToTable("rooms");
        table.HasKey(row => row.Number);
        table.Property(row => row.Number).HasColumnName("number");
        table.Property(row => row.Type).HasColumnName("type");
    });
}

// Reads the hotel's rooms from the rooms table. Only Program.cs creates it.
public class PostgresRoomCatalog(IDbContextFactory<RoomsDb> databases) : IRoomCatalog
{
    public IReadOnlyList<Room> All()
    {
        using var db = databases.CreateDbContext();
        return db.Rooms.Select(row => new Room(row.Number, row.Type)).ToList();
    }
}
