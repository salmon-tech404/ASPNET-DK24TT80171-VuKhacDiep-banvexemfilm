using System;
using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class DatVe
{
    public int Id { get; set; }
    public string MaGiaoDich { get; set; } = string.Empty;
    public int MaNguoiDung { get; set; }
    public int MaSuatChieu { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThaiDat { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed
    public DateTime NgayDat { get; set; } = DateTime.Now;
    public DateTime NgayCapNhat { get; set; } = DateTime.Now;

    // Navigation
    public NguoiDung NguoiDung { get; set; } = null!;
    public SuatChieu SuatChieu { get; set; } = null!;
    public ICollection<ChiTietGheDat> ChiTietGheDats { get; set; } = new List<ChiTietGheDat>();
    public ThanhToan? ThanhToan { get; set; }
}
