using cinemaBooking.Models.Domain;

namespace cinemaBooking.Services.Interfaces;

public interface ICinemaService
{
    Task<List<Cinema>> GetAllCinemasAsync();
    Task<Cinema?> GetCinemaByIdAsync(int cinemaId);
    Task<List<Room>> GetRoomsByCinemaAsync(int cinemaId);
    Task<Room?> GetRoomByIdAsync(int roomId);
    Task<List<(Room Room, Cinema Cinema)>> GetAllRoomsWithCinemaAsync();
}
