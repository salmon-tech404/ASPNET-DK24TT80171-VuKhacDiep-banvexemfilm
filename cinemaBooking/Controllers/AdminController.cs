using cinemaBooking.Data;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly IShowtimeService _showtimeService;
    private readonly IMovieService _movieService;
    private readonly ICinemaService _cinemaService;
    private readonly CinemaDbContext _context;

    public AdminController(IUserService userService, IBookingService bookingService,
        IShowtimeService showtimeService, IMovieService movieService,
        ICinemaService cinemaService, CinemaDbContext context)
    {
        _userService = userService;
        _bookingService = bookingService;
        _showtimeService = showtimeService;
        _movieService = movieService;
        _cinemaService = cinemaService;
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var allBookings = await _bookingService.GetAllBookingsAsync();
        var confirmedBookings = allBookings.Where(b => b.BookingStatus == "Confirmed").ToList();

        var topMovies = confirmedBookings
            .GroupBy(b => b.MovieTitle)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new TopMovieViewModel
            {
                Title = g.Key,
                BookingCount = g.Count(),
                Revenue = g.Sum(b => b.TotalAmount)
            }).ToList();

        var vm = new AdminDashboardViewModel
        {
            TotalMovies = await _context.Movies.CountAsync(),
            NowShowingMovies = await _context.Movies.CountAsync(m => m.Status == "NowShowing"),
            TotalUsers = await _context.Users.CountAsync(u => u.Role != "Admin"),
            TotalBookings = allBookings.Count,
            PendingBookings = allBookings.Count(b => b.BookingStatus == "Pending"),
            TotalRevenue = confirmedBookings.Sum(b => b.TotalAmount),
            RecentBookings = allBookings.Take(10).Select(b => new RecentBookingViewModel
            {
                Id = b.BookingId,
                BookingCode = b.BookingCode,
                MovieTitle = b.MovieTitle,
                TotalAmount = b.TotalAmount,
                Status = b.BookingStatus,
                CreatedAt = b.CreatedAt
            }).ToList(),
            TopMovies = topMovies
        };
        return View(vm);
    }

    public async Task<IActionResult> Users(string? search, string? role)
    {
        var users = await _userService.GetAllUsersAsync(search, role);
        ViewBag.Search = search;
        ViewBag.Role = role;
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleUserStatus(int id)
    {
        await _userService.ToggleUserActiveAsync(id);
        TempData["Success"] = "Cập nhật trạng thái người dùng thành công.";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserRole(int id, string role)
    {
        await _userService.ChangeUserRoleAsync(id, role);
        TempData["Success"] = "Cập nhật vai trò người dùng thành công.";
        return RedirectToAction(nameof(Users));
    }

    public async Task<IActionResult> Movies(string? search, string? status)
    {
        var movies = await _movieService.GetMoviesAsync(search, status);
        ViewBag.Search = search;
        ViewBag.Status = status;
        return View(movies);
    }

    public async Task<IActionResult> Showtimes()
    {
        var showtimes = await _showtimeService.GetAllShowtimesAsync();
        return View(showtimes);
    }

    [HttpGet]
    public async Task<IActionResult> CreateShowtime()
    {
        var vm = await _showtimeService.GetShowtimeFormAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateShowtime(ShowtimeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var freshVm = await _showtimeService.GetShowtimeFormAsync();
            model.Movies = freshVm.Movies;
            model.Rooms = freshVm.Rooms;
            return View(model);
        }

        await _showtimeService.CreateShowtimeAsync(model);
        TempData["Success"] = "Thêm suất chiếu thành công.";
        return RedirectToAction(nameof(Showtimes));
    }

    [HttpGet]
    public async Task<IActionResult> EditShowtime(int id)
    {
        var vm = await _showtimeService.GetShowtimeFormAsync(id);
        if (vm.Id == 0) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditShowtime(int id, ShowtimeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var freshVm = await _showtimeService.GetShowtimeFormAsync();
            model.Movies = freshVm.Movies;
            model.Rooms = freshVm.Rooms;
            return View(model);
        }

        await _showtimeService.UpdateShowtimeAsync(id, model);
        TempData["Success"] = "Cập nhật suất chiếu thành công.";
        return RedirectToAction(nameof(Showtimes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteShowtime(int id)
    {
        await _showtimeService.DeleteShowtimeAsync(id);
        TempData["Success"] = "Đã hủy suất chiếu.";
        return RedirectToAction(nameof(Showtimes));
    }

    public async Task<IActionResult> Bookings(string? status)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(status);
        ViewBag.Status = status;
        return View(bookings);
    }

    public async Task<IActionResult> Cinemas()
    {
        var cinemas = await _cinemaService.GetAllCinemasAsync();
        return View(cinemas);
    }
}
