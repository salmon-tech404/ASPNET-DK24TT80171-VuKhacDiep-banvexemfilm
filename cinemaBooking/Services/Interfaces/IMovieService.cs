using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;

namespace cinemaBooking.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieCardViewModel>> GetMoviesAsync(string? search = null, string? status = null, int? genreId = null);
    Task<MovieDetailViewModel?> GetMovieDetailAsync(int movieId, DateTime? date = null);
    Task<Phim?> GetMovieByIdAsync(int movieId);
    Task<Phim> CreateMovieAsync(MovieFormViewModel model);
    Task<bool> UpdateMovieAsync(int movieId, MovieFormViewModel model);
    Task<bool> DeleteMovieAsync(int movieId);
    Task<List<TheLoai>> GetAllGenresAsync();
    Task<MovieFormViewModel> GetMovieFormAsync(int? movieId = null);
}
