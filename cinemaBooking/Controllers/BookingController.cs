using System.Security.Claims;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cinemaBooking.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IShowtimeService _showtimeService;

    public BookingController(IBookingService bookingService, IShowtimeService showtimeService)
    {
        _bookingService = bookingService;
        _showtimeService = showtimeService;
    }

    [HttpGet]
    public async Task<IActionResult> SeatSelection(int showtimeId)
    {
        var vm = await _showtimeService.GetSeatSelectionAsync(showtimeId);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        if (!ModelState.IsValid || !model.SelectedSeatIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn ít nhất 1 ghế.";
            return RedirectToAction(nameof(SeatSelection), new { showtimeId = model.ShowtimeId });
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await _bookingService.CreateBookingAsync(userId, model);

        if (booking == null)
        {
            TempData["Error"] = "Đặt vé thất bại. Ghế đã được đặt hoặc không hợp lệ.";
            return RedirectToAction(nameof(SeatSelection), new { showtimeId = model.ShowtimeId });
        }

        return RedirectToAction(nameof(Confirmation), new { id = booking.BookingId });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await _bookingService.GetBookingDetailAsync(id, userId);
        if (booking == null) return NotFound();
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _bookingService.ProcessPaymentAsync(id, userId);

        if (result)
            TempData["Success"] = "Thanh toán thành công! Vé đã được xác nhận.";
        else
            TempData["Error"] = "Thanh toán thất bại. Vui lòng thử lại.";

        return RedirectToAction(nameof(Confirmation), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return View(bookings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _bookingService.CancelBookingAsync(id, userId);

        if (result)
            TempData["Success"] = "Đã hủy vé thành công.";
        else
            TempData["Error"] = "Không thể hủy vé này.";

        return RedirectToAction(nameof(History));
    }
}
