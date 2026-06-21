using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class CinemaService : ICinemaService
{
    private readonly CinemaDbContext _context;

    public CinemaService(CinemaDbContext context) => _context = context;

    public async Task<List<RapChieu>> GetAllCinemasAsync() =>
        await _context.RapChieu.OrderBy(c => c.TenRap).ToListAsync();

    public async Task<RapChieu?> GetCinemaByIdAsync(int cinemaId) =>
        await _context.RapChieu.Include(c => c.PhongChieus).FirstOrDefaultAsync(c => c.Id == cinemaId);

    public async Task<List<PhongChieu>> GetRoomsByCinemaAsync(int cinemaId) =>
        await _context.PhongChieu.Where(r => r.MaRap == cinemaId && r.TrangThai).ToListAsync();

    public async Task<PhongChieu?> GetRoomByIdAsync(int roomId) =>
        await _context.PhongChieu.Include(r => r.RapChieu).FirstOrDefaultAsync(r => r.Id == roomId);

    public async Task<List<(PhongChieu Room, RapChieu Cinema)>> GetAllRoomsWithCinemaAsync()
    {
        var rooms = await _context.PhongChieu
            .Include(r => r.RapChieu)
            .Where(r => r.TrangThai)
            .OrderBy(r => r.RapChieu.TenRap).ThenBy(r => r.TenPhong)
            .ToListAsync();
        return rooms.Select(r => (r, r.RapChieu)).ToList();
    }
}
