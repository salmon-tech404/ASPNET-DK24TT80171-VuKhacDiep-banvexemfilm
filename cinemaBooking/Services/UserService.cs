using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class UserService : IUserService
{
    private readonly CinemaDbContext _context;

    public UserService(CinemaDbContext context) => _context = context;

    public async Task<NguoiDung?> GetUserByIdAsync(int id) =>
        await _context.NguoiDung.FindAsync(id);

    public async Task<NguoiDung?> GetUserByEmailAsync(string email) =>
        await _context.NguoiDung.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<NguoiDung?> AuthenticateAsync(string email, string password)
    {
        var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Email == email && u.TrangThai);
        if (user == null) return null;
        return user.MatKhau == password ? user : null;
    }

    public async Task<NguoiDung?> RegisterAsync(string fullName, string email, string password, string? phone)
    {
        if (await EmailExistsAsync(email)) return null;

        var user = new NguoiDung
        {
            HoTen = fullName,
            Email = email,
            MatKhau = password,
            DienThoai = phone,
            VaiTro = "Customer",
            TrangThai = true,
            NgayTao = DateTime.Now,
            NgayCapNhat = DateTime.Now
        };
        _context.NguoiDung.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateProfileAsync(int userId, string fullName, string? phone, string? avatarUrl)
    {
        var user = await _context.NguoiDung.FindAsync(userId);
        if (user == null) return false;

        user.HoTen = fullName;
        user.DienThoai = phone;
        user.AnhDaiDien = avatarUrl;
        user.NgayCapNhat = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _context.NguoiDung.FindAsync(userId);
        if (user == null) return false;
        if (user.MatKhau != currentPassword) return false;

        user.MatKhau = newPassword;
        user.NgayCapNhat = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<NguoiDung>> GetAllUsersAsync(string? search = null, string? role = null)
    {
        var query = _context.NguoiDung.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.HoTen.Contains(search) || u.Email.Contains(search));

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(u => u.VaiTro == role);

        return await query.OrderByDescending(u => u.NgayTao).ToListAsync();
    }

    public async Task<bool> ToggleUserActiveAsync(int userId)
    {
        var user = await _context.NguoiDung.FindAsync(userId);
        if (user == null) return false;
        user.TrangThai = !user.TrangThai;
        user.NgayCapNhat = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeUserRoleAsync(int userId, string role)
    {
        var user = await _context.NguoiDung.FindAsync(userId);
        if (user == null) return false;
        user.VaiTro = role;
        user.NgayCapNhat = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.NguoiDung.AnyAsync(u => u.Email == email);
}
