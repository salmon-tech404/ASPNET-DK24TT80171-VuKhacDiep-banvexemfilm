using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class ShowtimeFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phim")]
    public int MaPhim { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phòng")]
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Thời gian bắt đầu không được để trống")]
    public DateTime GioBatDau { get; set; }

    public DateTime GioKetThuc { get; set; }

    public string PhongDich { get; set; } = "Vietsub";
    public string DinhDang { get; set; } = "2D";

    [Required]
    [Range(1000, 10000000)]
    public decimal GiaVeCoBan { get; set; }

    public string TrangThaiSuat { get; set; } = "Scheduled";

    // Select lists
    public List<(int Id, string TenPhim)> Phims { get; set; } = new();
    public List<(int Id, string TenPhong, int MaRap, string TenRap)> Phongs { get; set; } = new();
}

public class ShowtimeAdminListViewModel
{
    public int Id { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string TenRap { get; set; } = string.Empty;
    public string TenPhong { get; set; } = string.Empty;
    public DateTime GioBatDau { get; set; }
    public string DinhDang { get; set; } = "2D";
    public string PhongDich { get; set; } = "Vietsub";
    public decimal GiaVeCoBan { get; set; }
    public string TrangThaiSuat { get; set; } = "Scheduled";
    public int SoLuongDatVe { get; set; }
}
