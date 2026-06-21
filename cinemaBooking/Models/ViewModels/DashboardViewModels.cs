namespace cinemaBooking.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TongSoPhim { get; set; }
    public int PhimDangChieu { get; set; }
    public int TongSoNguoiDung { get; set; }
    public int TongSoDatVe { get; set; }
    public int DatVePending { get; set; }
    public decimal TongDoanhThu { get; set; }
    public List<RecentBookingViewModel> DanhSachDatVeGanDay { get; set; } = new();
    public List<TopMovieViewModel> DanhSachPhimTop { get; set; } = new();
}

public class RecentBookingViewModel
{
    public int Id { get; set; }
    public string MaGiaoDich { get; set; } = string.Empty;
    public string TenNguoiDung { get; set; } = string.Empty;
    public string TenPhim { get; set; } = string.Empty;
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = "Pending";
    public DateTime NgayTao { get; set; }
}

public class TopMovieViewModel
{
    public int MaPhim { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? AnhPoster { get; set; }
    public int SoLuongDatVe { get; set; }
    public decimal DoanhThu { get; set; }
}

public class HomeViewModel
{
    public List<MovieCardViewModel> PhimDangChieu { get; set; } = new();
    public List<MovieCardViewModel> PhimSapChieu { get; set; } = new();
}
