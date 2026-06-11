namespace cinemaBooking.Models.Domain;

public class Room
{
    public int Id { get; set; }
    public int CinemaId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public string RoomType { get; set; } = "Standard"; // Standard, VIP, 4DX, IMAX
    public bool IsActive { get; set; } = true;

    // Navigation
    public Cinema Cinema { get; set; } = null!;
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
