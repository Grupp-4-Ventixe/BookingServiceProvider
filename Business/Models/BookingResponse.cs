namespace Business.Models;

public class BookingResponse
{
    public string Id { get; set; } = null!;
    public Guid EventId { get; set; }
    public int TicketQuantity { get; set; } = 1;
    public DateTime BookingDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
