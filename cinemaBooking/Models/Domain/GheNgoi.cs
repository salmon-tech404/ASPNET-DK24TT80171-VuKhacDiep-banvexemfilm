using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class GheNgoi
{
    public int Id { get; set; }
    public int MaPhong { get; set; }
    public string HangGhe { get; set; } = string.Empty; // A, B, C...
    public int SoGhe { get; set; }
    public string LoaiGhe { get; set; } = "Regular"; // Regular, VIP, Couple
    public bool TrangThai { get; set; } = true;

    // Navigation
    public PhongChieu PhongChieu { get; set; } = null!;
    public ICollection<ChiTietGheDat> ChiTietGheDats { get; set; } = new List<ChiTietGheDat>();
}
