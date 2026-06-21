using cinemaBooking.Models.Domain;

namespace cinemaBooking.Services.Interfaces;

public interface IUserService
{
    Task<NguoiDung?> GetUserByIdAsync(int id);
    Task<NguoiDung?> GetUserByEmailAsync(string email);
    Task<NguoiDung?> AuthenticateAsync(string email, string password);
    Task<NguoiDung?> RegisterAsync(string fullName, string email, string password, string? phone);
    Task<bool> UpdateProfileAsync(int userId, string fullName, string? phone, string? avatarUrl);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<List<NguoiDung>> GetAllUsersAsync(string? search = null, string? role = null);
    Task<bool> ToggleUserActiveAsync(int userId);
    Task<bool> ChangeUserRoleAsync(int userId, string role);
    Task<bool> EmailExistsAsync(string email);
}
