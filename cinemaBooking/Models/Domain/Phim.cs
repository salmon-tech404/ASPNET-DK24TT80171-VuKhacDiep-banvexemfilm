using System;
using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class Phim
{
    public int Id { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? TenGoc { get; set; }
    public string? MoTa { get; set; }
    public int ThoiLuong { get; set; } // minutes
    public DateOnly? NgayChieu { get; set; }
    public DateOnly? NgayKetThuc { get; set; }
    public string? NgonNgu { get; set; }
    public string? QuocGia { get; set; }
    public string? DaoDien { get; set; }
    public string? DienVien { get; set; }
    public string? AnhPoster { get; set; }
    public string? LinkTrailer { get; set; }
    public decimal DiemDanhGia { get; set; } = 0;
    public string DoTuoiQuyDinh { get; set; } = "P"; // P, C13, C16, C18
    public string TrangThaiChieu { get; set; } = "ComingSoon"; // NowShowing, ComingSoon, Ended
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public DateTime NgayCapNhat { get; set; } = DateTime.Now;

    // Navigation
    public ICollection<TheLoaiPhim> TheLoaiPhims { get; set; } = new List<TheLoaiPhim>();
    public ICollection<SuatChieu> SuatChieus { get; set; } = new List<SuatChieu>();
}
