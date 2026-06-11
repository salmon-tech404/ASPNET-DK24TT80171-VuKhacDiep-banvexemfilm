namespace cinemaBooking.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalMovies { get; set; }
    public int NowShowingMovies { get; set; }
    public int TotalUsers { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<RecentBookingViewModel> RecentBookings { get; set; } = new();
    public List<TopMovieViewModel> TopMovies { get; set; } = new();
}

public class RecentBookingViewModel
{
    public int Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}

public class TopMovieViewModel
{
    public int MovieId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public int BookingCount { get; set; }
    public decimal Revenue { get; set; }
}

public class HomeViewModel
{
    public List<MovieCardViewModel> NowShowingMovies { get; set; } = new();
    public List<MovieCardViewModel> ComingSoonMovies { get; set; } = new();
}
