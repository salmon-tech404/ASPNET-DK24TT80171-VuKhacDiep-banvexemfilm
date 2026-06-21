using System;
using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class SuatChieu
{
    public int Id { get; set; }
    public int MaPhim { get; set; }
    public int MaPhong { get; set; }
    public DateTime GioBatDau { get; set; }
    public DateTime GioKetThuc { get; set; }
    public string PhongDich { get; set; } = "Vietsub"; // Vietsub, Dubbed, Original
    public string DinhDang { get; set; } = "2D"; // 2D, 3D, 4DX, IMAX
    public decimal GiaVeCoBan { get; set; }
    public string TrangThaiSuat { get; set; } = "Scheduled"; // Scheduled, Ongoing, Completed, Cancelled
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Navigation
    public Phim Phim { get; set; } = null!;
    public PhongChieu PhongChieu { get; set; } = null!;
    public ICollection<DatVe> DatVes { get; set; } = new List<DatVe>();
}
