using System;
using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class NguoiDung
{
    public int Id { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? AnhDaiDien { get; set; }
    public string VaiTro { get; set; } = "Customer"; // Admin, Customer
    public bool TrangThai { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime NgayCapNhat { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<DatVe> DatVes { get; set; } = new List<DatVe>();
}
