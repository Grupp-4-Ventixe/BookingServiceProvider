using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Business.Models;
using Presentation.Filters;
using Swashbuckle.AspNetCore.Annotations;
using Data.Entities;

namespace Presentation.Controllers;
//Tagit hjälp av ChatGPT för SwaggerDoc
[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingsService = bookingService;

    /// <summary>
    /// Create a new booking.
    /// </summary>
    [HttpPost]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Create a new booking", Description = "Adds a new booking to the booking list.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _bookingsService.CreateBookingAsync(request);
        return result.Success
            ? Ok("Booking added to booking list successfully.")
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error ?? "Unable to create booking.");
    }

    /// <summary>
    /// Get all bookings.
    /// </summary>
    [HttpGet]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Get all bookings", Description = "Returns a list of all bookings in the system.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BookingResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingsService.GetAllBookingsAsync();
        return result.Success
            ? Ok(result.Result)
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error);
    }

    /// <summary>
    /// Get bookings by email.
    /// </summary>
    [HttpGet("byemail")]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Get bookings by email", Description = "Fetch bookings based on user email.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookingEntity>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var result = await _bookingsService.GetBookingsByEmailAsync(email);
        return result.Success
            ? Ok(result.Result)
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error);
    }

    /// <summary>
    /// Delete a booking by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Delete a booking", Description = "Deletes a booking including its owner and address.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _bookingsService.DeleteFullBookingAsync(id);
        return result.Success
            ? Ok("Booking deleted.")
            : NotFound(result.Error);
    }
}