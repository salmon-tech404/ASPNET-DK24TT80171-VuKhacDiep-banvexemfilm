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
        var showtime = await _context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Room).ThenInclude(r => r.Cinema)
            .FirstOrDefaultAsync(s => s.Id == model.ShowtimeId);

        if (showtime == null) return null;

        // Check seats belong to the showtime's room and are not booked
        var bookedSeatIds = (await _context.BookingSeats
            .Include(bs => bs.Booking)
            .Where(bs => bs.Booking.ShowtimeId == model.ShowtimeId && bs.Booking.Status != "Cancelled")
            .Select(bs => bs.SeatId)
            .ToListAsync())
            .ToHashSet();

        var seats = await _context.Seats
            .Where(s => model.SelectedSeatIds.Contains(s.Id) && s.RoomId == showtime.RoomId && s.IsActive)
            .ToListAsync();

        if (seats.Count != model.SelectedSeatIds.Count) return null;
        if (seats.Any(s => bookedSeatIds.Contains(s.Id))) return null;

        decimal total = 0;
        var bookingSeats = seats.Select(s =>
        {
            var price = CalculateSeatPrice(showtime.BasePrice, s.SeatType, showtime.Format);
            total += price;
            return new BookingSeat { SeatId = s.Id, Price = price };
        }).ToList();

        var booking = new Booking
        {
            BookingCode = GenerateBookingCode(),
            UserId = userId,
            ShowtimeId = model.ShowtimeId,
            TotalAmount = total,
            Status = "Pending",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        foreach (var bs in bookingSeats)
        {
            bs.BookingId = booking.Id;
            _context.BookingSeats.Add(bs);
        }

        _context.Payments.Add(new Payment
        {
            BookingId = booking.Id,
            Method = model.PaymentMethod,
            Amount = total,
            Status = "Pending",
            CreatedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();

        return await GetBookingDetailAsync(booking.Id, userId);
    }

    public async Task<bool> ProcessPaymentAsync(int bookingId, int userId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null || booking.Status != "Pending") return false;

        booking.Status = "Confirmed";
        booking.UpdatedAt = DateTime.Now;

        if (booking.Payment != null)
        {
            booking.Payment.Status = "Success";
            booking.Payment.TransactionCode = "TXN" + booking.BookingCode;
            booking.Payment.PaidAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BookingConfirmViewModel?> GetBookingDetailAsync(int bookingId, int userId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Showtime).ThenInclude(s => s.Movie)
            .Include(b => b.Showtime).ThenInclude(s => s.Room).ThenInclude(r => r.Cinema)
            .Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat)
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null) return null;

        return new BookingConfirmViewModel
        {
            BookingId = booking.Id,
            BookingCode = booking.BookingCode,
            MovieTitle = booking.Showtime.Movie.Title,
            MoviePoster = booking.Showtime.Movie.PosterUrl,
            ShowtimeStart = booking.Showtime.StartTime,
            Format = booking.Showtime.Format,
            SubType = booking.Showtime.SubType,
            CinemaName = booking.Showtime.Room.Cinema.Name,
            RoomName = booking.Showtime.Room.Name,
            Seats = booking.BookingSeats.Select(bs => $"{bs.Seat.RowLabel}{bs.Seat.SeatNumber} ({bs.Seat.SeatType})").ToList(),
            TotalAmount = booking.TotalAmount,
            BookingStatus = booking.Status,
            PaymentStatus = booking.Payment?.Status ?? "Pending",
            PaymentMethod = booking.Payment?.Method ?? "Mock",
            PaidAt = booking.Payment?.PaidAt
        };
    }

    public async Task<List<BookingHistoryItemViewModel>> GetUserBookingsAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Showtime).ThenInclude(s => s.Movie)
            .Include(b => b.Showtime).ThenInclude(s => s.Room).ThenInclude(r => r.Cinema)
            .Include(b => b.BookingSeats)
            .Include(b => b.Payment)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookingHistoryItemViewModel
            {
                BookingId = b.Id,
                BookingCode = b.BookingCode,
                MovieTitle = b.Showtime.Movie.Title,
                MoviePoster = b.Showtime.Movie.PosterUrl,
                ShowtimeStart = b.Showtime.StartTime,
                CinemaName = b.Showtime.Room.Cinema.Name,
                SeatCount = b.BookingSeats.Count,
                TotalAmount = b.TotalAmount,
                BookingStatus = b.Status,
                PaymentStatus = b.Payment != null ? b.Payment.Status : "Pending",
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<BookingHistoryItemViewModel>> GetAllBookingsAsync(string? status = null)
    {
        var query = _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Showtime).ThenInclude(s => s.Movie)
            .Include(b => b.Showtime).ThenInclude(s => s.Room).ThenInclude(r => r.Cinema)
            .Include(b => b.BookingSeats)
            .Include(b => b.Payment)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.Status == status);

        return await query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookingHistoryItemViewModel
            {
                BookingId = b.Id,
                BookingCode = b.BookingCode,
                MovieTitle = b.Showtime.Movie.Title,
                MoviePoster = b.Showtime.Movie.PosterUrl,
                ShowtimeStart = b.Showtime.StartTime,
                CinemaName = b.Showtime.Room.Cinema.Name,
                SeatCount = b.BookingSeats.Count,
                TotalAmount = b.TotalAmount,
                BookingStatus = b.Status,
                PaymentStatus = b.Payment != null ? b.Payment.Status : "Pending",
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> CancelBookingAsync(int bookingId, int userId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null || booking.Status == "Cancelled") return false;

        booking.Status = "Cancelled";
        booking.UpdatedAt = DateTime.Now;

        if (booking.Payment?.Status == "Success")
            booking.Payment.Status = "Refunded";

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
            "4DX" or "IMAX" => 1.5m,
            _ => 1.0m
        };
        return Math.Round(price / 1000) * 1000;
    }
}
