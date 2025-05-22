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

  
    }
