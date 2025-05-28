using Xunit;
using Moq;
using FluentAssertions;
using Business.Services;
using Data.Entities;
using Data.Repositories;
using Business.Models;
using Data;
using System.Linq.Expressions;

namespace BookingTests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock = new();
    private readonly Mock<IBookingOwnerRepository> _ownerRepoMock = new();
    private readonly Mock<IBookingAddressRepository> _addressRepoMock = new();
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _service = new BookingService(_bookingRepoMock.Object, _ownerRepoMock.Object, _addressRepoMock.Object);
    }

    [Fact]
    public async Task CreateBookingAsync_Should_Return_Success_When_Valid()
    {
        var request = new CreateBookingRequest
        {
            EventId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            StreetName = "Main St",
            PostalCode = "12345",
            City = "Cityville",
            TicketQuantity = 2
        };

        _bookingRepoMock.Setup(repo => repo.AddAsync(It.IsAny<BookingEntity>()))
            .ReturnsAsync(new RepositoryResult<bool> { Success = true });

        var result = await _service.CreateBookingAsync(request);

        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task CreateBookingAsync_Should_Return_Failure_When_Add_Fails()
    {
        var request = new CreateBookingRequest
        {
            EventId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            StreetName = "Main St",
            PostalCode = "12345",
            City = "Cityville",
            TicketQuantity = 2
        };

        _bookingRepoMock.Setup(repo => repo.AddAsync(It.IsAny<BookingEntity>()))
            .ReturnsAsync(new RepositoryResult<bool> { Success = false, Error = "DB Error" });

        var result = await _service.CreateBookingAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().Be("DB Error");
    }

    [Fact]
    public async Task GetBookingsByEmailAsync_Should_Return_Result_When_Found()
    {
        var bookings = new List<BookingEntity> {
            new BookingEntity { Id = "b1", EventId = Guid.NewGuid(), TicketQuantity = 1 }
        };

        _bookingRepoMock.Setup(repo => repo.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(new RepositoryResult<IEnumerable<BookingEntity>> { Success = true, Result = bookings });

        var result = await _service.GetBookingsByEmailAsync("test@example.com");

        result.Success.Should().BeTrue();
        result.Result.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteFullBookingAsync_Should_Delete_When_Exists()
    {
        var bookingId = "b1";
        var owner = new BookingOwnerEntity
        {
            Id = "o1",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            Address = new BookingAddressEntity
            {
                Id = "a1",
                StreetName = "Main St",
                City = "City",
                PostalCode = "12345"
            }
        };
        var booking = new BookingEntity { Id = bookingId, BookingOwner = owner };

        _bookingRepoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<BookingEntity, bool>>>()))!
            .ReturnsAsync(new RepositoryResult<BookingEntity> { Success = true, Result = booking });
        _bookingRepoMock.Setup(r => r.DeleteAsync(booking))
            .ReturnsAsync(new RepositoryResult<bool> { Success = true });
        _ownerRepoMock.Setup(r => r.DeleteAsync(owner))
            .ReturnsAsync(new RepositoryResult<bool> { Success = true });
        _addressRepoMock.Setup(r => r.DeleteAsync(owner.Address))
            .ReturnsAsync(new RepositoryResult<bool> { Success = true });

        var result = await _service.DeleteFullBookingAsync(bookingId);

        result.Success.Should().BeTrue();
    }
    

    }
