using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class MovieService : IMovieService
{
    private readonly CinemaDbContext _context;

    public MovieService(CinemaDbContext context) => _context = context;

    public async Task<List<MovieCardViewModel>> GetMoviesAsync(string? search = null, string? status = null, int? genreId = null)
    {
        var query = _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Title.Contains(search) || (m.OriginalTitle != null && m.OriginalTitle.Contains(search)));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(m => m.Status == status);

        if (genreId.HasValue)
            query = query.Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId.Value));

        var movies = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        return movies.Select(ToCardViewModel).ToList();
    }

    public async Task<MovieDetailViewModel?> GetMovieDetailAsync(int movieId, DateTime? date = null)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.Showtimes)
                .ThenInclude(s => s.Room)
                    .ThenInclude(r => r.Cinema)
            .Include(m => m.Showtimes)
                .ThenInclude(s => s.Bookings)
                    .ThenInclude(b => b.BookingSeats)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null) return null;

        var filterDate = date?.Date ?? DateTime.Today;
        var nextDate = filterDate.AddDays(1);

        var filteredShowtimes = movie.Showtimes
            .Where(s => s.Status != "Cancelled" && s.StartTime >= filterDate && s.StartTime < nextDate)
            .ToList();

        var cinemaGroups = filteredShowtimes
            .GroupBy(s => s.Room.Cinema)
            .Select(g => new ShowtimeGroupViewModel
            {
                CinemaName = g.Key.Name,
                CinemaCity = g.Key.City,
                Showtimes = g.Select(s =>
                {
                    var bookedSeats = s.Bookings
                        .Where(b => b.Status != "Cancelled")
                        .SelectMany(b => b.BookingSeats)
                        .Count();
                    return new ShowtimeItemViewModel
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Format = s.Format,
                        SubType = s.SubType,
                        BasePrice = s.BasePrice,
                        RoomName = s.Room.Name,
                        Status = s.Status,
                        AvailableSeats = s.Room.TotalSeats - bookedSeats,
                        TotalSeats = s.Room.TotalSeats
                    };
                }).OrderBy(s => s.StartTime).ToList()
            }).ToList();

        return new MovieDetailViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            OriginalTitle = movie.OriginalTitle,
            Description = movie.Description,
            Duration = movie.Duration,
            ReleaseDate = movie.ReleaseDate,
            EndDate = movie.EndDate,
            Language = movie.Language,
            Country = movie.Country,
            Director = movie.Director,
            Cast = movie.Cast,
            PosterUrl = movie.PosterUrl,
            TrailerUrl = movie.TrailerUrl,
            Rating = movie.Rating,
            AgeRating = movie.AgeRating,
            Status = movie.Status,
            Genres = movie.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
            ShowtimeGroups = cinemaGroups
        };
    }

    public async Task<Movie?> GetMovieByIdAsync(int movieId) =>
        await _context.Movies.Include(m => m.MovieGenres).FirstOrDefaultAsync(m => m.Id == movieId);

    public async Task<Movie> CreateMovieAsync(MovieFormViewModel model)
    {
        var movie = new Movie
        {
            Title = model.Title,
            OriginalTitle = model.OriginalTitle,
            Description = model.Description,
            Duration = model.Duration,
            ReleaseDate = model.ReleaseDate,
            EndDate = model.EndDate,
            Language = model.Language,
            Country = model.Country,
            Director = model.Director,
            Cast = model.Cast,
            PosterUrl = model.PosterUrl,
            TrailerUrl = model.TrailerUrl,
            Rating = model.Rating,
            AgeRating = model.AgeRating,
            Status = model.Status,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        foreach (var genreId in model.SelectedGenreIds)
            _context.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateMovieAsync(int movieId, MovieFormViewModel model)
    {
        var movie = await _context.Movies.Include(m => m.MovieGenres).FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null) return false;

        movie.Title = model.Title;
        movie.OriginalTitle = model.OriginalTitle;
        movie.Description = model.Description;
        movie.Duration = model.Duration;
        movie.ReleaseDate = model.ReleaseDate;
        movie.EndDate = model.EndDate;
        movie.Language = model.Language;
        movie.Country = model.Country;
        movie.Director = model.Director;
        movie.Cast = model.Cast;
        movie.PosterUrl = model.PosterUrl;
        movie.TrailerUrl = model.TrailerUrl;
        movie.Rating = model.Rating;
        movie.AgeRating = model.AgeRating;
        movie.Status = model.Status;
        movie.UpdatedAt = DateTime.Now;

        _context.MovieGenres.RemoveRange(movie.MovieGenres);
        foreach (var genreId in model.SelectedGenreIds)
            _context.MovieGenres.Add(new MovieGenre { MovieId = movieId, GenreId = genreId });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie == null) return false;
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Genre>> GetAllGenresAsync() =>
        await _context.Genres.OrderBy(g => g.Name).ToListAsync();

    public async Task<MovieFormViewModel> GetMovieFormAsync(int? movieId = null)
    {
        var allGenres = await GetAllGenresAsync();
        var vm = new MovieFormViewModel
        {
            AllGenres = allGenres.Select(g => new GenreCheckboxItem { Id = g.Id, Name = g.Name }).ToList()
        };

        if (movieId.HasValue)
        {
            var movie = await GetMovieByIdAsync(movieId.Value);
            if (movie != null)
            {
                vm.Id = movie.Id;
                vm.Title = movie.Title;
                vm.OriginalTitle = movie.OriginalTitle;
                vm.Description = movie.Description;
                vm.Duration = movie.Duration;
                vm.ReleaseDate = movie.ReleaseDate;
                vm.EndDate = movie.EndDate;
                vm.Language = movie.Language;
                vm.Country = movie.Country;
                vm.Director = movie.Director;
                vm.Cast = movie.Cast;
                vm.PosterUrl = movie.PosterUrl;
                vm.TrailerUrl = movie.TrailerUrl;
                vm.Rating = movie.Rating;
                vm.AgeRating = movie.AgeRating;
                vm.Status = movie.Status;
                vm.SelectedGenreIds = movie.MovieGenres.Select(mg => mg.GenreId).ToList();

                foreach (var item in vm.AllGenres)
                    item.IsChecked = vm.SelectedGenreIds.Contains(item.Id);
            }
        }

        return vm;
    }

    private static MovieCardViewModel ToCardViewModel(Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        PosterUrl = movie.PosterUrl,
        Duration = movie.Duration,
        Rating = movie.Rating,
        AgeRating = movie.AgeRating,
        Status = movie.Status,
        ReleaseDate = movie.ReleaseDate,
        Genres = movie.MovieGenres.Select(mg => mg.Genre.Name).ToList()
    };
}
