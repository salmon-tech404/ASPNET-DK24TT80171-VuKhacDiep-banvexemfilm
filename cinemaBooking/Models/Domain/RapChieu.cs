using System;
using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class RapChieu
{
    public int Id { get; set; }
    public string TenRap { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
    public string ThanhPho { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? Email { get; set; }
    public string? HinhAnhRap { get; set; }
    public bool TrangThai { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<PhongChieu> PhongChieus { get; set; } = new List<PhongChieu>();
}
