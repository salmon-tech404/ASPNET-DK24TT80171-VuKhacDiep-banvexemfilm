namespace cinemaBooking.Models.Domain;

public class ChiTietGheDat
{
    public int Id { get; set; }
    public int MaDatVe { get; set; }
    public int MaGhe { get; set; }
    public decimal GiaVeThucTe { get; set; }

    // Navigation
    public DatVe DatVe { get; set; } = null!;
    public GheNgoi GheNgoi { get; set; } = null!;
}
