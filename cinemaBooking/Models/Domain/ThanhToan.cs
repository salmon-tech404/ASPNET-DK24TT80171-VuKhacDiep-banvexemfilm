using System;

namespace cinemaBooking.Models.Domain;

public class ThanhToan
{
    public int Id { get; set; }
    public int MaDatVe { get; set; }
    public string PhuongThuc { get; set; } = "Mock"; // Mock, VNPay, MoMo
    public decimal SoTien { get; set; }
    public string? MaGiaoDichNganHang { get; set; }
    public string TrangThaiThanhToan { get; set; } = "Pending"; // Pending, Success, Failed, Refunded
    public DateTime? NgayThanhToan { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Navigation
    public DatVe DatVe { get; set; } = null!;
}
