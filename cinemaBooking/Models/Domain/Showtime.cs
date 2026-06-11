namespace cinemaBooking.Models.Domain;

public class Showtime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string SubType { get; set; } = "Vietsub"; // Vietsub, Dubbed, Original
    public string Format { get; set; } = "2D"; // 2D, 3D, 4DX, IMAX
    public decimal BasePrice { get; set; }
    public string Status { get; set; } = "Scheduled"; // Scheduled, Ongoing, Completed, Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation
    public Movie Movie { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
