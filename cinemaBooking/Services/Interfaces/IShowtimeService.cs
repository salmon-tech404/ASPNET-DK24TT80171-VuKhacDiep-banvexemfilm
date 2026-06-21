using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;

namespace cinemaBooking.Services.Interfaces;

public interface IShowtimeService
{
    Task<List<ShowtimeAdminListViewModel>> GetAllShowtimesAsync();
    Task<SuatChieu?> GetShowtimeByIdAsync(int showtimeId);
    Task<SeatSelectionViewModel?> GetSeatSelectionAsync(int showtimeId);
    Task<SuatChieu> CreateShowtimeAsync(ShowtimeFormViewModel model);
    Task<bool> UpdateShowtimeAsync(int showtimeId, ShowtimeFormViewModel model);
    Task<bool> DeleteShowtimeAsync(int showtimeId);
    Task<List<int>> GetBookedSeatIdsAsync(int showtimeId);
    Task<ShowtimeFormViewModel> GetShowtimeFormAsync(int? showtimeId = null);
}
