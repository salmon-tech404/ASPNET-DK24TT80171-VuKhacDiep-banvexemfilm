using System.ComponentModel.DataAnnotations;

namespace cinemaBooking.Models.ViewModels;

public class SeatSelectionViewModel
{
    public int ShowtimeId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string? MoviePoster { get; set; }
    public DateTime StartTime { get; set; }
    public string Format { get; set; } = "2D";
    public string SubType { get; set; } = "Vietsub";
    public string CinemaName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public string RoomType { get; set; } = "Standard";
    public decimal BasePrice { get; set; }
    public List<SeatRowViewModel> SeatRows { get; set; } = new();
    public List<int> BookedSeatIds { get; set; } = new();
}

public class SeatRowViewModel
{
    public string RowLabel { get; set; } = string.Empty;
    public List<SeatViewModel> Seats { get; set; } = new();
}

public class SeatViewModel
{
    public int Id { get; set; }
    public string RowLabel { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = "Regular"; // Regular, VIP, Couple
    public bool IsBooked { get; set; }
    public bool IsActive { get; set; }
    public decimal Price { get; set; }
}

public class CreateBookingViewModel
{
    [Required]
    public int ShowtimeId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ít nhất 1 ghế")]
    public List<int> SelectedSeatIds { get; set; } = new();

    public string PaymentMethod { get; set; } = "Mock"; // Mock, VNPay, MoMo
}

public class BookingConfirmViewModel
{
    public int BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public string? MoviePoster { get; set; }
    public DateTime ShowtimeStart { get; set; }
    public string Format { get; set; } = "2D";
    public string SubType { get; set; } = "Vietsub";
    public string CinemaName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public List<string> Seats { get; set; } = new(); // "A1 (Regular)", "B5 (VIP)"
    public decimal TotalAmount { get; set; }
    public string BookingStatus { get; set; } = "Pending";
    public string PaymentStatus { get; set; } = "Pending";
    public string PaymentMethod { get; set; } = "Mock";
    public DateTime? PaidAt { get; set; }
}

public class BookingHistoryItemViewModel
{
    public int BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public string? MoviePoster { get; set; }
    public DateTime ShowtimeStart { get; set; }
    public string CinemaName { get; set; } = string.Empty;
    public int SeatCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string BookingStatus { get; set; } = "Pending";
    public string PaymentStatus { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
