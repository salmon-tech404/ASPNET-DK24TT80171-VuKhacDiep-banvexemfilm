namespace cinemaBooking.Models.Domain;

public class Payment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public string Method { get; set; } = "Mock"; // Mock, VNPay, MoMo
    public decimal Amount { get; set; }
    public string? TransactionCode { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Refunded
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public Booking Booking { get; set; } = null!;
}
