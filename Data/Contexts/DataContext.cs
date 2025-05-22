using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<BookingOwnerEntity> BookingOwners { get; set; }
    public DbSet<BookingAddressEntity> BookingOwnerAddresses { get; set; }
}
