using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class SeatSelectionViewModel
{
    public int MaSuatChieu { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? AnhPoster { get; set; }
    public DateTime GioBatDau { get; set; }
    public string DinhDang { get; set; } = "2D";
    public string PhongDich { get; set; } = "Vietsub";
    public string TenRap { get; set; } = string.Empty;
    public string TenPhong { get; set; } = string.Empty;
    public string LoaiPhong { get; set; } = "Standard";
    public decimal GiaVeCoBan { get; set; }
    public List<SeatRowViewModel> HangGhes { get; set; } = new();
    public List<int> DanhSachGheDaDat { get; set; } = new();
}

public class SeatRowViewModel
{
    public string HangGhe { get; set; } = string.Empty;
    public List<SeatViewModel> GheNgois { get; set; } = new();
}

public class SeatViewModel
{
    public int Id { get; set; }
    public string HangGhe { get; set; } = string.Empty;
    public int SoGhe { get; set; }
    public string LoaiGhe { get; set; } = "Regular"; // Regular, VIP, Couple
    public bool DaDat { get; set; }
    public bool TrangThai { get; set; }
    public decimal GiaVe { get; set; }
}

public class CreateBookingViewModel
{
    [Required]
    public int MaSuatChieu { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ít nhất 1 ghế")]
    public List<int> DanhSachMaGheChon { get; set; } = new();

    public string PhuongThucThanhToan { get; set; } = "Mock"; // Mock, VNPay, MoMo
}

public class BookingConfirmViewModel
{
    public int MaDatVe { get; set; }
    public string MaGiaoDich { get; set; } = string.Empty;
    public string TenPhim { get; set; } = string.Empty;
    public string? AnhPoster { get; set; }
    public DateTime GioBatDau { get; set; }
    public string DinhDang { get; set; } = "2D";
    public string PhongDich { get; set; } = "Vietsub";
    public string TenRap { get; set; } = string.Empty;
    public string TenPhong { get; set; } = string.Empty;
    public List<string> DanhSachGhe { get; set; } = new(); // "A1 (Regular)", "B5 (VIP)"
    public decimal TongTien { get; set; }
    public string TrangThaiDat { get; set; } = "Pending";
    public string TrangThaiThanhToan { get; set; } = "Pending";
    public string PhuongThucThanhToan { get; set; } = "Mock";
    public DateTime? NgayThanhToan { get; set; }
}

public class BookingHistoryItemViewModel
{
    public int MaDatVe { get; set; }
    public string MaGiaoDich { get; set; } = string.Empty;
    public string TenPhim { get; set; } = string.Empty;
    public string? AnhPoster { get; set; }
    public DateTime GioBatDau { get; set; }
    public string TenRap { get; set; } = string.Empty;
    public int SoLuongGhe { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThaiDat { get; set; } = "Pending";
    public string TrangThaiThanhToan { get; set; } = "Pending";
    public DateTime NgayTao { get; set; }
}
