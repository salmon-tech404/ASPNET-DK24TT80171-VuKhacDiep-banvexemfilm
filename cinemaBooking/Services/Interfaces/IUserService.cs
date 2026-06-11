using cinemaBooking.Models.Domain;

namespace cinemaBooking.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User?> RegisterAsync(string fullName, string email, string password, string? phone);
    Task<bool> UpdateProfileAsync(int userId, string fullName, string? phone, string? avatarUrl);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<List<User>> GetAllUsersAsync(string? search = null, string? role = null);
    Task<bool> ToggleUserActiveAsync(int userId);
    Task<bool> ChangeUserRoleAsync(int userId, string role);
    Task<bool> EmailExistsAsync(string email);
}
