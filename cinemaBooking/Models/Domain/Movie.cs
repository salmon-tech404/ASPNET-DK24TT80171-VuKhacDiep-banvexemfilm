namespace cinemaBooking.Models.Domain;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; } // minutes
    public DateOnly? ReleaseDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; } // comma-separated
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public decimal Rating { get; set; } = 0;
    public string AgeRating { get; set; } = "P"; // P, C13, C16, C18
    public string Status { get; set; } = "ComingSoon"; // NowShowing, ComingSoon, Ended
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
