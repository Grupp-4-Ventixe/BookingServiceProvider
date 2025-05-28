using Xunit;
using FluentAssertions;
using Data.Repositories;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Contexts;
namespace BookingTests.Repositories;

public class BookingRepositoryTests
{
    private readonly DataContext _context;
    private readonly BookingRepository _repository;

    public BookingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _repository = new BookingRepository(_context);

        SeedData();
    }

    private void SeedData()
    {
        var booking = new BookingEntity
        {
            Id = Guid.NewGuid().ToString(),
            EventId = Guid.NewGuid(),
            BookingDate = DateTime.Now,
            TicketQuantity = 2,
            BookingOwner = new BookingOwnerEntity
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Address = new BookingAddressEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    StreetName = "Gatan 1",
                    PostalCode = "12345",
                    City = "Teststad"
                }
            }
        };

        _context.Bookings.Add(booking);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Return_Correct_Booking()
    {
        var result = await _repository.GetByEmailAsync("test@example.com");

        result.Success.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.Count().Should().Be(1);
        result.Result.First().BookingOwner.Email.Should().Be("test@example.com");
    }
}