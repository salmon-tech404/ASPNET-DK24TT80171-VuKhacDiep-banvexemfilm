using System;

namespace cinemaBooking.Models.Domain;

public class BannerQuangCao
{
    public int Id { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string DuongDanAnh { get; set; } = string.Empty;
    public string? DuongDanLienKet { get; set; }
    public bool TrangThai { get; set; } = true;
    public int ThuTuHienThi { get; set; } = 0;
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
