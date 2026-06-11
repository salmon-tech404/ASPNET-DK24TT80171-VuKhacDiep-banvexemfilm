using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class ShowtimeFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phim")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phòng")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Thời gian bắt đầu không được để trống")]
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string SubType { get; set; } = "Vietsub";
    public string Format { get; set; } = "2D";

    [Required]
    [Range(1000, 10000000)]
    public decimal BasePrice { get; set; }

    public string Status { get; set; } = "Scheduled";

    // Select lists
    public List<(int Id, string Title)> Movies { get; set; } = new();
    public List<(int Id, string DisplayName, int CinemaId, string CinemaName)> Rooms { get; set; } = new();
}

public class ShowtimeAdminListViewModel
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string CinemaName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string Format { get; set; } = "2D";
    public string SubType { get; set; } = "Vietsub";
    public decimal BasePrice { get; set; }
    public string Status { get; set; } = "Scheduled";
    public int BookingsCount { get; set; }
}
