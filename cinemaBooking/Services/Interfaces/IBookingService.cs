using cinemaBooking.Models.ViewModels;

namespace cinemaBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingConfirmViewModel?> CreateBookingAsync(int userId, CreateBookingViewModel model);
    Task<bool> ProcessPaymentAsync(int bookingId, int userId);
    Task<BookingConfirmViewModel?> GetBookingDetailAsync(int bookingId, int userId);
    Task<List<BookingHistoryItemViewModel>> GetUserBookingsAsync(int userId);
    Task<List<BookingHistoryItemViewModel>> GetAllBookingsAsync(string? status = null);
    Task<bool> CancelBookingAsync(int bookingId, int userId);
}
