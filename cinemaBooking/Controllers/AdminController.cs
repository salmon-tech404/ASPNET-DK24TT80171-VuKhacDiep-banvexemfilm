using cinemaBooking.Data;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Controllers;

[Authorize(Roles = "Admin")]
[Route("quan-tri")]
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

    [Route("bang-dieu-khien")]
    public async Task<IActionResult> Dashboard()
    {
        var allBookings = await _bookingService.GetAllBookingsAsync();
        var confirmedBookings = allBookings.Where(b => b.TrangThaiDat == "Confirmed").ToList();

        var topMovies = confirmedBookings
            .GroupBy(b => b.TenPhim)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new TopMovieViewModel
            {
                TenPhim = g.Key,
                SoLuongDatVe = g.Count(),
                DoanhThu = g.Sum(b => b.TongTien)
            }).ToList();

        var vm = new AdminDashboardViewModel
        {
            TongSoPhim = await _context.Phim.CountAsync(),
            PhimDangChieu = await _context.Phim.CountAsync(m => m.TrangThaiChieu == "NowShowing"),
            TongSoNguoiDung = await _context.NguoiDung.CountAsync(u => u.VaiTro != "Admin"),
            TongSoDatVe = allBookings.Count,
            DatVePending = allBookings.Count(b => b.TrangThaiDat == "Pending"),
            TongDoanhThu = confirmedBookings.Sum(b => b.TongTien),
            DanhSachDatVeGanDay = allBookings.Take(10).Select(b => new RecentBookingViewModel
            {
                Id = b.MaDatVe,
                MaGiaoDich = b.MaGiaoDich,
                TenPhim = b.TenPhim,
                TongTien = b.TongTien,
                TrangThai = b.TrangThaiDat,
                NgayTao = b.NgayTao
            }).ToList(),
            DanhSachPhimTop = topMovies
        };
        return View(vm);
    }

    [Route("nguoi-dung")]
    public async Task<IActionResult> Users(string? search, string? role)
    {
        var users = await _userService.GetAllUsersAsync(search, role);
        ViewBag.Search = search;
        ViewBag.Role = role;
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("trang-thai-nguoi-dung/{id}")]
    public async Task<IActionResult> ToggleUserStatus(int id)
    {
        await _userService.ToggleUserActiveAsync(id);
        TempData["Success"] = "Cập nhật trạng thái người dùng thành công.";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("doi-vai-tro-nguoi-dung/{id}")]
    public async Task<IActionResult> ChangeUserRole(int id, string role)
    {
        await _userService.ChangeUserRoleAsync(id, role);
        TempData["Success"] = "Cập nhật vai trò người dùng thành công.";
        return RedirectToAction(nameof(Users));
    }

    [Route("phim")]
    public async Task<IActionResult> Movies(string? search, string? status)
    {
        var movies = await _movieService.GetMoviesAsync(search, status);
        ViewBag.Search = search;
        ViewBag.Status = status;
        return View(movies);
    }

    [Route("suat-chieu")]
    public async Task<IActionResult> Showtimes()
    {
        var showtimes = await _showtimeService.GetAllShowtimesAsync();
        return View(showtimes);
    }

    [HttpGet]
    [Route("them-suat-chieu")]
    public async Task<IActionResult> CreateShowtime()
    {
        var vm = await _showtimeService.GetShowtimeFormAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("them-suat-chieu")]
    public async Task<IActionResult> CreateShowtime(ShowtimeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var freshVm = await _showtimeService.GetShowtimeFormAsync();
            model.Phims = freshVm.Phims;
            model.Phongs = freshVm.Phongs;
            return View(model);
        }

        await _showtimeService.CreateShowtimeAsync(model);
        TempData["Success"] = "Thêm suất chiếu thành công.";
        return RedirectToAction(nameof(Showtimes));
    }

    [HttpGet]
    [Route("sua-suat-chieu/{id}")]
    public async Task<IActionResult> EditShowtime(int id)
    {
        var vm = await _showtimeService.GetShowtimeFormAsync(id);
        if (vm.Id == 0) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("sua-suat-chieu/{id}")]
    public async Task<IActionResult> EditShowtime(int id, ShowtimeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var freshVm = await _showtimeService.GetShowtimeFormAsync();
            model.Phims = freshVm.Phims;
            model.Phongs = freshVm.Phongs;
            return View(model);
        }

        await _showtimeService.UpdateShowtimeAsync(id, model);
        TempData["Success"] = "Cập nhật suất chiếu thành công.";
        return RedirectToAction(nameof(Showtimes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("xoa-suat-chieu/{id}")]
    public async Task<IActionResult> DeleteShowtime(int id)
    {
        await _showtimeService.DeleteShowtimeAsync(id);
        TempData["Success"] = "Đã hủy suất chiếu.";
        return RedirectToAction(nameof(Showtimes));
    }

    [Route("dat-ve")]
    public async Task<IActionResult> Bookings(string? status)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(status);
        ViewBag.Status = status;
        return View(bookings);
    }

    [Route("rap-chieu")]
    public async Task<IActionResult> Cinemas()
    {
        var cinemas = await _cinemaService.GetAllCinemasAsync();
        return View(cinemas);
    }
}
