namespace cinemaBooking.Models.Domain;

public class Seat
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RowLabel { get; set; } = string.Empty; // A, B, C...
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = "Regular"; // Regular, VIP, Couple
    public bool IsActive { get; set; } = true;

    // Navigation
    public Room Room { get; set; } = null!;
    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
}
