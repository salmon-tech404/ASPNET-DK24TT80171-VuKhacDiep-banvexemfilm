using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class MovieCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public int Duration { get; set; }
    public decimal Rating { get; set; }
    public string AgeRating { get; set; } = "P";
    public string Status { get; set; } = "ComingSoon";
    public DateOnly? ReleaseDate { get; set; }
    public List<string> Genres { get; set; } = new();
}

public class MovieDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public decimal Rating { get; set; }
    public string AgeRating { get; set; } = "P";
    public string Status { get; set; } = "ComingSoon";
    public List<string> Genres { get; set; } = new();
    public List<ShowtimeGroupViewModel> ShowtimeGroups { get; set; } = new();
}

public class ShowtimeGroupViewModel
{
    public string CinemaName { get; set; } = string.Empty;
    public string CinemaCity { get; set; } = string.Empty;
    public List<ShowtimeItemViewModel> Showtimes { get; set; } = new();
}

public class ShowtimeItemViewModel
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Format { get; set; } = "2D";
    public string SubType { get; set; } = "Vietsub";
    public decimal BasePrice { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
    public int AvailableSeats { get; set; }
    public int TotalSeats { get; set; }
}

public class MovieFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phim không được để trống")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? OriginalTitle { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Thời lượng không được để trống")]
    [Range(1, 500)]
    public int Duration { get; set; }

    public DateOnly? ReleaseDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }

    [Range(0, 10)]
    public decimal Rating { get; set; }

    public string AgeRating { get; set; } = "P";
    public string Status { get; set; } = "ComingSoon";

    public List<int> SelectedGenreIds { get; set; } = new();
    public List<GenreCheckboxItem> AllGenres { get; set; } = new();
}

public class GenreCheckboxItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
}

public class MovieListPageViewModel
{
    public List<MovieCardViewModel> Movies { get; set; } = new();
    public string? SearchQuery { get; set; }
    public string? StatusFilter { get; set; }
    public int? GenreFilter { get; set; }
    public List<(int Id, string Name)> Genres { get; set; } = new();
}
