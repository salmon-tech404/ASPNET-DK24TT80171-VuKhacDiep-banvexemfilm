namespace cinemaBooking.Models.Domain;

public class TheLoaiPhim
{
    public int MaPhim { get; set; }
    public int MaTheLoai { get; set; }

    // Navigation
    public Phim Phim { get; set; } = null!;
    public TheLoai TheLoai { get; set; } = null!;
}
