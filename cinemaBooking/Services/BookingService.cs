using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class BookingService : IBookingService
{
    private readonly CinemaDbContext _context;

    public BookingService(CinemaDbContext context) => _context = context;

    public async Task<BookingConfirmViewModel?> CreateBookingAsync(int userId, CreateBookingViewModel model)
    {
        var showtime = await _context.SuatChieu
            .Include(s => s.Phim)
            .Include(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .FirstOrDefaultAsync(s => s.Id == model.MaSuatChieu);

        if (showtime == null) return null;

        // Check seats belong to the showtime's room and are not booked
        var bookedSeatIds = (await _context.ChiTietGheDat
            .Include(bs => bs.DatVe)
            .Where(bs => bs.DatVe.MaSuatChieu == model.MaSuatChieu && bs.DatVe.TrangThaiDat != "Cancelled")
            .Select(bs => bs.MaGhe)
            .ToListAsync())
            .ToHashSet();

        var seats = await _context.GheNgoi
            .Where(s => model.DanhSachMaGheChon.Contains(s.Id) && s.MaPhong == showtime.MaPhong && s.TrangThai)
            .ToListAsync();

        if (seats.Count != model.DanhSachMaGheChon.Count) return null;
        if (seats.Any(s => bookedSeatIds.Contains(s.Id))) return null;

        decimal total = 0;
        var bookingSeats = seats.Select(s =>
        {
            var price = CalculateSeatPrice(showtime.GiaVeCoBan, s.LoaiGhe, showtime.DinhDang);
            total += price;
            return new ChiTietGheDat { MaGhe = s.Id, GiaVeThucTe = price };
        }).ToList();

        var booking = new DatVe
        {
            MaGiaoDich = GenerateBookingCode(),
            MaNguoiDung = userId,
            MaSuatChieu = model.MaSuatChieu,
            TongTien = total,
            TrangThaiDat = "Pending",
            NgayDat = DateTime.Now,
            NgayCapNhat = DateTime.Now
        };
        _context.DatVe.Add(booking);
        await _context.SaveChangesAsync();

        foreach (var bs in bookingSeats)
        {
            bs.MaDatVe = booking.Id;
            _context.ChiTietGheDat.Add(bs);
        }

        _context.ThanhToan.Add(new ThanhToan
        {
            MaDatVe = booking.Id,
            PhuongThuc = model.PhuongThucThanhToan,
            SoTien = total,
            TrangThaiThanhToan = "Pending",
            NgayTao = DateTime.Now
        });
        await _context.SaveChangesAsync();

        return await GetBookingDetailAsync(booking.Id, userId);
    }

    public async Task<bool> ProcessPaymentAsync(int bookingId, int userId)
    {
        var booking = await _context.DatVe
            .Include(b => b.ThanhToan)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.MaNguoiDung == userId);

        if (booking == null || booking.TrangThaiDat != "Pending") return false;

        booking.TrangThaiDat = "Confirmed";
        booking.NgayCapNhat = DateTime.Now;

        if (booking.ThanhToan != null)
        {
            booking.ThanhToan.TrangThaiThanhToan = "Success";
            booking.ThanhToan.MaGiaoDichNganHang = "TXN" + booking.MaGiaoDich;
            booking.ThanhToan.NgayThanhToan = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BookingConfirmViewModel?> GetBookingDetailAsync(int bookingId, int userId)
    {
        var booking = await _context.DatVe
            .Include(b => b.SuatChieu).ThenInclude(s => s.Phim)
            .Include(b => b.SuatChieu).ThenInclude(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .Include(b => b.ChiTietGheDats).ThenInclude(bs => bs.GheNgoi)
            .Include(b => b.ThanhToan)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.MaNguoiDung == userId);

        if (booking == null) return null;

        return new BookingConfirmViewModel
        {
            MaDatVe = booking.Id,
            MaGiaoDich = booking.MaGiaoDich,
            TenPhim = booking.SuatChieu.Phim.TenPhim,
            AnhPoster = booking.SuatChieu.Phim.AnhPoster,
            GioBatDau = booking.SuatChieu.GioBatDau,
            DinhDang = booking.SuatChieu.DinhDang,
            PhongDich = booking.SuatChieu.PhongDich,
            TenRap = booking.SuatChieu.PhongChieu.RapChieu.TenRap,
            TenPhong = booking.SuatChieu.PhongChieu.TenPhong,
            DanhSachGhe = booking.ChiTietGheDats.Select(bs => $"{bs.GheNgoi.HangGhe}{bs.GheNgoi.SoGhe} ({bs.GheNgoi.LoaiGhe})").ToList(),
            TongTien = booking.TongTien,
            TrangThaiDat = booking.TrangThaiDat,
            TrangThaiThanhToan = booking.ThanhToan?.TrangThaiThanhToan ?? "Pending",
            PhuongThucThanhToan = booking.ThanhToan?.PhuongThuc ?? "Mock",
            NgayThanhToan = booking.ThanhToan?.NgayThanhToan
        };
    }

    public async Task<List<BookingHistoryItemViewModel>> GetUserBookingsAsync(int userId)
    {
        return await _context.DatVe
            .Include(b => b.SuatChieu).ThenInclude(s => s.Phim)
            .Include(b => b.SuatChieu).ThenInclude(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .Include(b => b.ChiTietGheDats)
            .Include(b => b.ThanhToan)
            .Where(b => b.MaNguoiDung == userId)
            .OrderByDescending(b => b.NgayDat)
            .Select(b => new BookingHistoryItemViewModel
            {
                MaDatVe = b.Id,
                MaGiaoDich = b.MaGiaoDich,
                TenPhim = b.SuatChieu.Phim.TenPhim,
                AnhPoster = b.SuatChieu.Phim.AnhPoster,
                GioBatDau = b.SuatChieu.GioBatDau,
                TenRap = b.SuatChieu.PhongChieu.RapChieu.TenRap,
                SoLuongGhe = b.ChiTietGheDats.Count,
                TongTien = b.TongTien,
                TrangThaiDat = b.TrangThaiDat,
                TrangThaiThanhToan = b.ThanhToan != null ? b.ThanhToan.TrangThaiThanhToan : "Pending",
                NgayTao = b.NgayDat
            })
            .ToListAsync();
    }

    public async Task<List<BookingHistoryItemViewModel>> GetAllBookingsAsync(string? status = null)
    {
        var query = _context.DatVe
            .Include(b => b.NguoiDung)
            .Include(b => b.SuatChieu).ThenInclude(s => s.Phim)
            .Include(b => b.SuatChieu).ThenInclude(s => s.PhongChieu).ThenInclude(r => r.RapChieu)
            .Include(b => b.ChiTietGheDats)
            .Include(b => b.ThanhToan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.TrangThaiDat == status);

        return await query
            .OrderByDescending(b => b.NgayDat)
            .Select(b => new BookingHistoryItemViewModel
            {
                MaDatVe = b.Id,
                MaGiaoDich = b.MaGiaoDich,
                TenPhim = b.SuatChieu.Phim.TenPhim,
                AnhPoster = b.SuatChieu.Phim.AnhPoster,
                GioBatDau = b.SuatChieu.GioBatDau,
                TenRap = b.SuatChieu.PhongChieu.RapChieu.TenRap,
                SoLuongGhe = b.ChiTietGheDats.Count,
                TongTien = b.TongTien,
                TrangThaiDat = b.TrangThaiDat,
                TrangThaiThanhToan = b.ThanhToan != null ? b.ThanhToan.TrangThaiThanhToan : "Pending",
                NgayTao = b.NgayDat
            })
            .ToListAsync();
    }

    public async Task<bool> CancelBookingAsync(int bookingId, int userId)
    {
        var booking = await _context.DatVe
            .Include(b => b.ThanhToan)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.MaNguoiDung == userId);

        if (booking == null || booking.TrangThaiDat == "Cancelled") return false;

        booking.TrangThaiDat = "Cancelled";
        booking.NgayCapNhat = DateTime.Now;

        if (booking.ThanhToan?.TrangThaiThanhToan == "Success")
            booking.ThanhToan.TrangThaiThanhToan = "Refunded";

        await _context.SaveChangesAsync();
        return true;
    }

    private static string GenerateBookingCode() =>
        "BK" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(100, 999).ToString();

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
            _ => 1.0m
        };
        return Math.Round(price / 1000) * 1000;
    }
}
