using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class CinemaService : ICinemaService
{
    private readonly CinemaDbContext _context;

    public CinemaService(CinemaDbContext context) => _context = context;

    public async Task<List<Cinema>> GetAllCinemasAsync() =>
        await _context.Cinemas.OrderBy(c => c.Name).ToListAsync();

    public async Task<Cinema?> GetCinemaByIdAsync(int cinemaId) =>
        await _context.Cinemas.Include(c => c.Rooms).FirstOrDefaultAsync(c => c.Id == cinemaId);

    public async Task<List<Room>> GetRoomsByCinemaAsync(int cinemaId) =>
        await _context.Rooms.Where(r => r.CinemaId == cinemaId && r.IsActive).ToListAsync();

    public async Task<Room?> GetRoomByIdAsync(int roomId) =>
        await _context.Rooms.Include(r => r.Cinema).FirstOrDefaultAsync(r => r.Id == roomId);

    public async Task<List<(Room Room, Cinema Cinema)>> GetAllRoomsWithCinemaAsync()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Cinema)
            .Where(r => r.IsActive)
            .OrderBy(r => r.Cinema.Name).ThenBy(r => r.Name)
            .ToListAsync();
        return rooms.Select(r => (r, r.Cinema)).ToList();
    }
}
