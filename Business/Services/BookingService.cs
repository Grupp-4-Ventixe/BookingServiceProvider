using Data.Entities;
using Business.Models;
using Data.Repositories;

namespace Business.Services;

public interface IBookingService
{
    Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
    Task<BookingResult> DeleteFullBookingAsync(string bookingId);
    Task<BookingResult<List<BookingResponse>>> GetAllBookingsAsync();
    Task<BookingResult<IEnumerable<BookingEntity>>> GetBookingsByEmailAsync(string email);
}

public class BookingService(IBookingRepository bookingRepository, IBookingOwnerRepository bookingOwnerRepository, IBookingAddressRepository bookingAddressRepository ) : IBookingService
{

    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IBookingOwnerRepository _bookingOwnerRepository = bookingOwnerRepository;
    private readonly IBookingAddressRepository _bookingAddressRepository = bookingAddressRepository;

    public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
    {
        var bookingEntity = new BookingEntity
        {
            EventId = request.EventId,
            BookingDate = DateTime.Now,
            TicketQuantity = request.TicketQuantity,
            BookingOwner = new BookingOwnerEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = new BookingAddressEntity
                {
                    StreetName = request.StreetName,
                    PostalCode = request.PostalCode,
                    City = request.City,

                }
            }
        };

        var result = await _bookingRepository.AddAsync(bookingEntity);
        return result.Success
            ? new BookingResult { Success = true }
            : new BookingResult { Success = false, Error = result.Error };
    }

    public async Task<BookingResult<List<BookingResponse>>> GetAllBookingsAsync()
    {
        var result = await _bookingRepository.GetAllAsync();
        if (!result.Success || result.Result == null)
            return new BookingResult<List<BookingResponse>> { Success = false, Error = result.Error };

        var bookings = result.Result.Select(b => new BookingResponse
        {
            Id = b.Id,
            EventId = b.EventId,
            TicketQuantity = b.TicketQuantity,
            BookingDate = b.BookingDate,
            FirstName = b.BookingOwner.FirstName,
            LastName = b.BookingOwner.LastName,
            Email = b.BookingOwner.Email,
            StreetName = b.BookingOwner.Address?.StreetName ?? "",
            PostalCode = b.BookingOwner.Address?.PostalCode ?? "",
            City = b.BookingOwner.Address?.City ?? ""
        }).ToList();

        return new BookingResult<List<BookingResponse>> { Success = true, Result = bookings };
    }

    public async Task<BookingResult<IEnumerable<BookingEntity>>> GetBookingsByEmailAsync(string email)
    {
        var result = await _bookingRepository.GetByEmailAsync(email);
        return result.Success
            ? new BookingResult<IEnumerable<BookingEntity>> { Success = true, Result = result.Result }
            : new BookingResult<IEnumerable<BookingEntity>> { Success = false, Error = result.Error };
    }

    public async Task<BookingResult> DeleteFullBookingAsync(string bookingId)
    {
        var bookingResult = await _bookingRepository.GetAsync(x => x.Id == bookingId);
        if (!bookingResult.Success || bookingResult.Result == null)
            return new BookingResult { Success = false, Error = "Booking not found" };

        var booking = bookingResult.Result;

        var deleteBooking = await _bookingRepository.DeleteAsync(booking);
        if (!deleteBooking.Success)
            return new BookingResult { Success = false, Error = deleteBooking.Error };

        var deleteOwner = await _bookingOwnerRepository.DeleteAsync(booking.BookingOwner);
        if (!deleteOwner.Success)
            return new BookingResult { Success = false, Error = deleteOwner.Error };

        if (booking.BookingOwner.Address != null)
        {
            var deleteAddress = await _bookingAddressRepository.DeleteAsync(booking.BookingOwner.Address);
            if (!deleteAddress.Success)
                return new BookingResult { Success = false, Error = deleteAddress.Error };
        }

        return new BookingResult { Success = true };
    }


}



