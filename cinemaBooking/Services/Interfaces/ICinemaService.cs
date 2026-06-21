using cinemaBooking.Models.Domain;

namespace cinemaBooking.Services.Interfaces;

public interface ICinemaService
{
    Task<List<RapChieu>> GetAllCinemasAsync();
    Task<RapChieu?> GetCinemaByIdAsync(int cinemaId);
    Task<List<PhongChieu>> GetRoomsByCinemaAsync(int cinemaId);
    Task<PhongChieu?> GetRoomByIdAsync(int roomId);
    Task<List<(PhongChieu Room, RapChieu Cinema)>> GetAllRoomsWithCinemaAsync();
}
