using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Business.Models;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase{
    private readonly IBookingService _bookingsService = bookingService; 

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _bookingsService.CreateBookingAsync(request);
        return result.Success
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to create booking.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingsService.GetAllBookingsAsync();
        return result.Success
            ? Ok(result.Result)
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error);
    }

    [HttpGet("byemail")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var result = await _bookingsService.GetBookingsByEmailAsync(email);
        return result.Success
            ? Ok(result.Result)
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _bookingsService.DeleteFullBookingAsync(id);
        return result.Success
            ? Ok("Booking deleted.")
            : NotFound(result.Error);
    }

}
