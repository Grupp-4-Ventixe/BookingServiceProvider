using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<BookingOwnerEntity> BookingOwners { get; set; }
    public DbSet<BookingAddressEntity> BookingOwnerAddresses { get; set; }


    //Tagit hjälp av chatGPT 4o 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookingEntity>()
            .HasOne(b => b.BookingOwner)
            .WithMany()
            .HasForeignKey(b => b.BookingOwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingOwnerEntity>()
            .HasOne(o => o.Address)
            .WithMany()
            .HasForeignKey(o => o.BookingAddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

