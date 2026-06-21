using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class MovieCardViewModel
{
    public int Id { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? AnhPoster { get; set; }
    public int ThoiLuong { get; set; }
    public decimal DiemDanhGia { get; set; }
    public string DoTuoiQuyDinh { get; set; } = "P";
    public string TrangThaiChieu { get; set; } = "ComingSoon";
    public DateOnly? NgayChieu { get; set; }
    public List<string> TheLoais { get; set; } = new();
}

public class MovieDetailViewModel
{
    public int Id { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? TenGoc { get; set; }
    public string? MoTa { get; set; }
    public int ThoiLuong { get; set; }
    public DateOnly? NgayChieu { get; set; }
    public DateOnly? NgayKetThuc { get; set; }
    public string? NgonNgu { get; set; }
    public string? QuocGia { get; set; }
    public string? DaoDien { get; set; }
    public string? DienVien { get; set; }
    public string? AnhPoster { get; set; }
    public string? LinkTrailer { get; set; }
    public decimal DiemDanhGia { get; set; }
    public string DoTuoiQuyDinh { get; set; } = "P";
    public string TrangThaiChieu { get; set; } = "ComingSoon";
    public List<string> TheLoais { get; set; } = new();
    public List<ShowtimeGroupViewModel> NhomSuatChieus { get; set; } = new();
}

public class ShowtimeGroupViewModel
{
    public string TenRap { get; set; } = string.Empty;
    public string ThanhPho { get; set; } = string.Empty;
    public List<ShowtimeItemViewModel> SuatChieus { get; set; } = new();
}

public class ShowtimeItemViewModel
{
    public int Id { get; set; }
    public DateTime GioBatDau { get; set; }
    public DateTime GioKetThuc { get; set; }
    public string DinhDang { get; set; } = "2D";
    public string PhongDich { get; set; } = "Vietsub";
    public decimal GiaVeCoBan { get; set; }
    public string TenPhong { get; set; } = string.Empty;
    public string TrangThaiSuat { get; set; } = "Scheduled";
    public int SoGheTrong { get; set; }
    public int TongSoGhe { get; set; }
}

public class MovieFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên phim không được để trống")]
    [StringLength(200)]
    public string TenPhim { get; set; } = string.Empty;

    public string? TenGoc { get; set; }

    public string? MoTa { get; set; }

    [Required(ErrorMessage = "Thời lượng không được để trống")]
    [Range(1, 500)]
    public int ThoiLuong { get; set; }

    public DateOnly? NgayChieu { get; set; }
    public DateOnly? NgayKetThuc { get; set; }
    public string? NgonNgu { get; set; }
    public string? QuocGia { get; set; }
    public string? DaoDien { get; set; }
    public string? DienVien { get; set; }
    public string? AnhPoster { get; set; }
    public string? LinkTrailer { get; set; }

    [Range(0, 10)]
    public decimal DiemDanhGia { get; set; }

    public string DoTuoiQuyDinh { get; set; } = "P";
    public string TrangThaiChieu { get; set; } = "ComingSoon";

    public List<int> MaTheLoaisDaChon { get; set; } = new();
    public List<GenreCheckboxItem> TatCaTheLoai { get; set; } = new();
}

public class GenreCheckboxItem
{
    public int Id { get; set; }
    public string TenTheLoai { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
}

public class MovieListPageViewModel
{
    public List<MovieCardViewModel> Phims { get; set; } = new();
    public string? SearchQuery { get; set; }
    public string? StatusFilter { get; set; }
    public int? GenreFilter { get; set; }
    public List<(int Id, string TenTheLoai)> TheLoais { get; set; } = new();
}
