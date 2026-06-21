using System.Collections.Generic;

namespace cinemaBooking.Models.Domain;

public class TheLoai
{
    public int Id { get; set; }
    public string TenTheLoai { get; set; } = string.Empty;

    // Navigation
    public ICollection<TheLoaiPhim> TheLoaiPhims { get; set; } = new List<TheLoaiPhim>();
}
