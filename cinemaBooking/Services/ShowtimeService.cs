using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class ShowtimeService : IShowtimeService
{
    private readonly CinemaDbContext _context;
    private readonly IMovieService _movieService;
    private readonly ICinemaService _cinemaService;

    public ShowtimeService(CinemaDbContext context, IMovieService movieService, ICinemaService cinemaService)
    {
        _context = context;
        _movieService = movieService;
        _cinemaService = cinemaService;
    }

    public async Task<List<ShowtimeAdminListViewModel>> GetAllShowtimesAsync()
    {
        return await _context.SuatChieu
            .Include(s => s.Phim)
            .Include(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .Include(s => s.DatVes)
            .OrderByDescending(s => s.GioBatDau)
            .Select(s => new ShowtimeAdminListViewModel
            {
                Id = s.Id,
                TenPhim = s.Phim.TenPhim,
                TenRap = s.PhongChieu.RapChieu.TenRap,
                TenPhong = s.PhongChieu.TenPhong,
                GioBatDau = s.GioBatDau,
                DinhDang = s.DinhDang,
                PhongDich = s.PhongDich,
                GiaVeCoBan = s.GiaVeCoBan,
                TrangThaiSuat = s.TrangThaiSuat,
                SoLuongDatVe = s.DatVes.Count(b => b.TrangThaiDat != "Cancelled")
            })
            .ToListAsync();
    }

    public async Task<SuatChieu?> GetShowtimeByIdAsync(int showtimeId) =>
        await _context.SuatChieu
            .Include(s => s.Phim)
            .Include(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .FirstOrDefaultAsync(s => s.Id == showtimeId);

    public async Task<SeatSelectionViewModel?> GetSeatSelectionAsync(int showtimeId)
    {
        var showtime = await _context.SuatChieu
            .Include(s => s.Phim)
            .Include(s => s.PhongChieu).ThenInclude(r => r.GheNgois)
            .Include(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .Include(s => s.DatVes).ThenInclude(b => b.ChiTietGheDats)
            .FirstOrDefaultAsync(s => s.Id == showtimeId);

        if (showtime == null) return null;

        var bookedSeatIds = showtime.DatVes
            .Where(b => b.TrangThaiDat != "Cancelled")
            .SelectMany(b => b.ChiTietGheDats)
            .Select(bs => bs.MaGhe)
            .ToHashSet();

        var seatRows = showtime.PhongChieu.GheNgois
            .Where(s => s.TrangThai)
            .GroupBy(s => s.HangGhe)
            .OrderBy(g => g.Key)
            .Select(g => new SeatRowViewModel
            {
                HangGhe = g.Key,
                GheNgois = g.OrderBy(s => s.SoGhe)
                    .Select(s => new SeatViewModel
                    {
                        Id = s.Id,
                        HangGhe = s.HangGhe,
                        SoGhe = s.SoGhe,
                        LoaiGhe = s.LoaiGhe,
                        DaDat = bookedSeatIds.Contains(s.Id),
                        TrangThai = s.TrangThai,
                        GiaVe = CalculateSeatPrice(showtime.GiaVeCoBan, s.LoaiGhe, showtime.DinhDang)
                    }).ToList()
            }).ToList();

        return new SeatSelectionViewModel
        {
            MaSuatChieu = showtimeId,
            TenPhim = showtime.Phim.TenPhim,
            AnhPoster = showtime.Phim.AnhPoster,
            GioBatDau = showtime.GioBatDau,
            DinhDang = showtime.DinhDang,
            PhongDich = showtime.PhongDich,
            TenRap = showtime.PhongChieu.RapChieu.TenRap,
            TenPhong = showtime.PhongChieu.TenPhong,
            LoaiPhong = showtime.PhongChieu.LoaiPhong,
            GiaVeCoBan = showtime.GiaVeCoBan,
            HangGhes = seatRows,
            DanhSachGheDaDat = bookedSeatIds.ToList()
        };
    }

    public async Task<SuatChieu> CreateShowtimeAsync(ShowtimeFormViewModel model)
    {
        var movie = await _context.Phim.FindAsync(model.MaPhim);
        var endTime = model.GioBatDau.AddMinutes(movie?.ThoiLuong ?? 120);

        var showtime = new SuatChieu
        {
            MaPhim = model.MaPhim,
            MaPhong = model.MaPhong,
            GioBatDau = model.GioBatDau,
            GioKetThuc = endTime,
            PhongDich = model.PhongDich,
            DinhDang = model.DinhDang,
            GiaVeCoBan = model.GiaVeCoBan,
            TrangThaiSuat = model.TrangThaiSuat,
            NgayTao = DateTime.Now
        };
        _context.SuatChieu.Add(showtime);
        await _context.SaveChangesAsync();
        return showtime;
    }

    public async Task<bool> UpdateShowtimeAsync(int showtimeId, ShowtimeFormViewModel model)
    {
        var showtime = await _context.SuatChieu.FindAsync(showtimeId);
        if (showtime == null) return false;

        var movie = await _context.Phim.FindAsync(model.MaPhim);
        showtime.MaPhim = model.MaPhim;
        showtime.MaPhong = model.MaPhong;
        showtime.GioBatDau = model.GioBatDau;
        showtime.GioKetThuc = model.GioBatDau.AddMinutes(movie?.ThoiLuong ?? 120);
        showtime.PhongDich = model.PhongDich;
        showtime.DinhDang = model.DinhDang;
        showtime.GiaVeCoBan = model.GiaVeCoBan;
        showtime.TrangThaiSuat = model.TrangThaiSuat;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteShowtimeAsync(int showtimeId)
    {
        var showtime = await _context.SuatChieu.FindAsync(showtimeId);
        if (showtime == null) return false;
        showtime.TrangThaiSuat = "Cancelled";
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<int>> GetBookedSeatIdsAsync(int showtimeId)
    {
        return await _context.ChiTietGheDat
            .Include(bs => bs.DatVe)
            .Where(bs => bs.DatVe.MaSuatChieu == showtimeId && bs.DatVe.TrangThaiDat != "Cancelled")
            .Select(bs => bs.MaGhe)
            .ToListAsync();
    }

    public async Task<ShowtimeFormViewModel> GetShowtimeFormAsync(int? showtimeId = null)
    {
        var movies = await _context.Phim
            .Where(m => m.TrangThaiChieu != "Ended")
            .OrderBy(m => m.TenPhim)
            .ToListAsync();

        var rooms = await _cinemaService.GetAllRoomsWithCinemaAsync();

        var vm = new ShowtimeFormViewModel
        {
            Phims = movies.Select(m => (m.Id, m.TenPhim)).ToList(),
            Phongs = rooms.Select(r => (r.Room.Id, r.Room.TenPhong, r.Cinema.Id, r.Cinema.TenRap)).ToList()
        };

        if (showtimeId.HasValue)
        {
            var st = await _context.SuatChieu.FindAsync(showtimeId.Value);
            if (st != null)
            {
                vm.Id = st.Id;
                vm.MaPhim = st.MaPhim;
                vm.MaPhong = st.MaPhong;
                vm.GioBatDau = st.GioBatDau;
                vm.GioKetThuc = st.GioKetThuc;
                vm.PhongDich = st.PhongDich;
                vm.DinhDang = st.DinhDang;
                vm.GiaVeCoBan = st.GiaVeCoBan;
                vm.TrangThaiSuat = st.TrangThaiSuat;
            }
        }

        return vm;
    }

    private static decimal CalculateSeatPrice(decimal basePrice, string seatType, string format)
    {
        decimal price = basePrice;
        price *= seatType switch
        {
            "VIP" => 1.5m,
            "Couple" => 2.0m,
            _ => 1.0m
        };
        price *= format switch
        {
            "3D" => 1.2m,
            "4DX" or "IMAX" => 1.5m,
            _ => 1.0m
        };
        return Math.Round(price / 1000) * 1000;
    }
}
