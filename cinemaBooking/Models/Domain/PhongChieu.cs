using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class PhongChieu
{
    public int Id { get; set; }
    public int MaRap { get; set; }
    public string TenPhong { get; set; } = string.Empty;
    public int TongSoGhe { get; set; }
    public string LoaiPhong { get; set; } = "Standard"; // Standard, VIP, 4DX, IMAX
    public bool TrangThai { get; set; } = true;

    // Navigation
    public RapChieu RapChieu { get; set; } = null!;
    public ICollection<GheNgoi> GheNgois { get; set; } = new List<GheNgoi>();
    public ICollection<SuatChieu> SuatChieus { get; set; } = new List<SuatChieu>();
}
