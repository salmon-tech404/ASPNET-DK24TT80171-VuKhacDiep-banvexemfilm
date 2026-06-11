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
        return await _context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Room).ThenInclude(r => r.Cinema)
            .Include(s => s.Bookings)
            .OrderByDescending(s => s.StartTime)
            .Select(s => new ShowtimeAdminListViewModel
            {
                Id = s.Id,
                MovieTitle = s.Movie.Title,
                CinemaName = s.Room.Cinema.Name,
                RoomName = s.Room.Name,
                StartTime = s.StartTime,
                Format = s.Format,
                SubType = s.SubType,
                BasePrice = s.BasePrice,
                Status = s.Status,
                BookingsCount = s.Bookings.Count(b => b.Status != "Cancelled")
            })
            .ToListAsync();
    }

    public async Task<Showtime?> GetShowtimeByIdAsync(int showtimeId) =>
        await _context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Room).ThenInclude(r => r.Cinema)
            .FirstOrDefaultAsync(s => s.Id == showtimeId);

    public async Task<SeatSelectionViewModel?> GetSeatSelectionAsync(int showtimeId)
    {
        var showtime = await _context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Room).ThenInclude(r => r.Seats)
            .Include(s => s.Room).ThenInclude(r => r.Cinema)
            .Include(s => s.Bookings).ThenInclude(b => b.BookingSeats)
            .FirstOrDefaultAsync(s => s.Id == showtimeId);

        if (showtime == null) return null;

        var bookedSeatIds = showtime.Bookings
            .Where(b => b.Status != "Cancelled")
            .SelectMany(b => b.BookingSeats)
            .Select(bs => bs.SeatId)
            .ToHashSet();

        var seatRows = showtime.Room.Seats
            .Where(s => s.IsActive)
            .GroupBy(s => s.RowLabel)
            .OrderBy(g => g.Key)
            .Select(g => new SeatRowViewModel
            {
                RowLabel = g.Key,
                Seats = g.OrderBy(s => s.SeatNumber)
                    .Select(s => new SeatViewModel
                    {
                        Id = s.Id,
                        RowLabel = s.RowLabel,
                        SeatNumber = s.SeatNumber,
                        SeatType = s.SeatType,
                        IsBooked = bookedSeatIds.Contains(s.Id),
                        IsActive = s.IsActive,
                        Price = CalculateSeatPrice(showtime.BasePrice, s.SeatType, showtime.Format)
                    }).ToList()
            }).ToList();

        return new SeatSelectionViewModel
        {
            ShowtimeId = showtimeId,
            MovieTitle = showtime.Movie.Title,
            MoviePoster = showtime.Movie.PosterUrl,
            StartTime = showtime.StartTime,
            Format = showtime.Format,
            SubType = showtime.SubType,
            CinemaName = showtime.Room.Cinema.Name,
            RoomName = showtime.Room.Name,
            RoomType = showtime.Room.RoomType,
            BasePrice = showtime.BasePrice,
            SeatRows = seatRows,
            BookedSeatIds = bookedSeatIds.ToList()
        };
    }

    public async Task<Showtime> CreateShowtimeAsync(ShowtimeFormViewModel model)
    {
        var movie = await _context.Movies.FindAsync(model.MovieId);
        var endTime = model.StartTime.AddMinutes(movie?.Duration ?? 120);

        var showtime = new Showtime
        {
            MovieId = model.MovieId,
            RoomId = model.RoomId,
            StartTime = model.StartTime,
            EndTime = endTime,
            SubType = model.SubType,
            Format = model.Format,
            BasePrice = model.BasePrice,
            Status = model.Status,
            CreatedAt = DateTime.Now
        };
        _context.Showtimes.Add(showtime);
        await _context.SaveChangesAsync();
        return showtime;
    }

    public async Task<bool> UpdateShowtimeAsync(int showtimeId, ShowtimeFormViewModel model)
    {
        var showtime = await _context.Showtimes.FindAsync(showtimeId);
        if (showtime == null) return false;

        var movie = await _context.Movies.FindAsync(model.MovieId);
        showtime.MovieId = model.MovieId;
        showtime.RoomId = model.RoomId;
        showtime.StartTime = model.StartTime;
        showtime.EndTime = model.StartTime.AddMinutes(movie?.Duration ?? 120);
        showtime.SubType = model.SubType;
        showtime.Format = model.Format;
        showtime.BasePrice = model.BasePrice;
        showtime.Status = model.Status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteShowtimeAsync(int showtimeId)
    {
        var showtime = await _context.Showtimes.FindAsync(showtimeId);
        if (showtime == null) return false;
        showtime.Status = "Cancelled";
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<int>> GetBookedSeatIdsAsync(int showtimeId)
    {
        return await _context.BookingSeats
            .Include(bs => bs.Booking)
            .Where(bs => bs.Booking.ShowtimeId == showtimeId && bs.Booking.Status != "Cancelled")
            .Select(bs => bs.SeatId)
            .ToListAsync();
    }

    public async Task<ShowtimeFormViewModel> GetShowtimeFormAsync(int? showtimeId = null)
    {
        var movies = await _context.Movies
            .Where(m => m.Status != "Ended")
            .OrderBy(m => m.Title)
            .ToListAsync();

        var rooms = await _cinemaService.GetAllRoomsWithCinemaAsync();

        var vm = new ShowtimeFormViewModel
        {
            Movies = movies.Select(m => (m.Id, m.Title)).ToList(),
            Rooms = rooms.Select(r => (r.Room.Id, r.Room.Name, r.Cinema.Id, r.Cinema.Name)).ToList()
        };

        if (showtimeId.HasValue)
        {
            var st = await _context.Showtimes.FindAsync(showtimeId.Value);
            if (st != null)
            {
                vm.Id = st.Id;
                vm.MovieId = st.MovieId;
                vm.RoomId = st.RoomId;
                vm.StartTime = st.StartTime;
                vm.EndTime = st.EndTime;
                vm.SubType = st.SubType;
                vm.Format = st.Format;
                vm.BasePrice = st.BasePrice;
                vm.Status = st.Status;
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
