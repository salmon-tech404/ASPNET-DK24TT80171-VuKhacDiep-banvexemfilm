using cinemaBooking.Models.Domain;

namespace cinemaBooking.Data;

public static class SeedData
{
    public static void Initialize(CinemaDbContext context)
    {
        if (context.Users.Any()) return;

        // ===== USERS =====
        var users = new List<User>
        {
            new() { FullName = "Quản Trị Viên", Email = "admin@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Phone = "0900000001", Role = "Admin", IsActive = true, CreatedAt = DateTime.Now.AddDays(-90) },
            new() { FullName = "Vũ Khắc Diệp", Email = "diep@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"), Phone = "0900000002", Role = "Customer", IsActive = true, CreatedAt = DateTime.Now.AddDays(-60) },
            new() { FullName = "Nguyễn Thị Lan", Email = "lan@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"), Phone = "0900000003", Role = "Customer", IsActive = true, CreatedAt = DateTime.Now.AddDays(-50) },
            new() { FullName = "Trần Văn Hùng", Email = "hung@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"), Phone = "0900000004", Role = "Customer", IsActive = true, CreatedAt = DateTime.Now.AddDays(-45) },
            new() { FullName = "Phạm Minh Tuấn", Email = "tuan@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"), Phone = "0900000005", Role = "Customer", IsActive = true, CreatedAt = DateTime.Now.AddDays(-30) },
            new() { FullName = "Lê Thị Mai", Email = "mai@cinema.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"), Phone = "0900000006", Role = "Customer", IsActive = false, CreatedAt = DateTime.Now.AddDays(-20) },
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // ===== GENRES =====
        var genres = new List<Genre>
        {
            new() { Name = "Hành động", Slug = "hanh-dong" },
            new() { Name = "Hài hước", Slug = "hai-huoc" },
            new() { Name = "Kinh dị", Slug = "kinh-di" },
            new() { Name = "Tình cảm", Slug = "tinh-cam" },
            new() { Name = "Hoạt hình", Slug = "hoat-hinh" },
            new() { Name = "Khoa học viễn tưởng", Slug = "khoa-hoc-vien-tuong" },
            new() { Name = "Tâm lý", Slug = "tam-ly" },
            new() { Name = "Phiêu lưu", Slug = "phieu-luu" },
        };
        context.Genres.AddRange(genres);
        context.SaveChanges();

        // ===== MOVIES =====
        var movies = new List<Movie>
        {
            new() {
                Title = "Đất Rừng Phương Nam",
                OriginalTitle = "Southern Forest",
                Description = "Bộ phim mang đến cho khán giả hành trình khám phá vùng đất phương Nam hoang sơ, hùng vĩ với câu chuyện đầy cảm xúc về tình đất, tình người.",
                Duration = 115, ReleaseDate = new DateOnly(2024, 10, 1), EndDate = new DateOnly(2024, 12, 31),
                Language = "Tiếng Việt", Country = "Việt Nam", Director = "Nguyễn Quang Dũng",
                Cast = "Hứa Vĩ Văn, Tuấn Trần, Tiến Luật",
                PosterUrl = "https://upload.wikimedia.org/wikipedia/vi/4/4e/%C4%90%E1%BA%A5t_r%E1%BB%ABng_ph%C6%B0%C6%A1ng_Nam.jpg",
                Rating = 7.5m, AgeRating = "C13", Status = "NowShowing"
            },
            new() {
                Title = "Avengers: Secret Wars",
                OriginalTitle = "Avengers: Secret Wars",
                Description = "Các siêu anh hùng của Vũ trụ Marvel tập hợp một lần nữa để đối mặt với mối đe dọa lớn nhất từ trước đến nay.",
                Duration = 165, ReleaseDate = new DateOnly(2025, 5, 2),
                Language = "Tiếng Anh", Country = "Mỹ", Director = "Russo Brothers",
                Cast = "Robert Downey Jr., Chris Evans, Scarlett Johansson",
                PosterUrl = "https://images.unsplash.com/photo-1612036782180-6f0b6cd846fe?w=400",
                Rating = 8.5m, AgeRating = "C13", Status = "ComingSoon"
            },
            new() {
                Title = "Mai",
                OriginalTitle = "Mai",
                Description = "Câu chuyện tình yêu đầy cảm xúc giữa hai con người có hoàn cảnh khác nhau, vượt qua những rào cản của cuộc sống để tìm đến hạnh phúc.",
                Duration = 130, ReleaseDate = new DateOnly(2024, 2, 10), EndDate = new DateOnly(2024, 4, 30),
                Language = "Tiếng Việt", Country = "Việt Nam", Director = "Trấn Thành",
                Cast = "Phương Anh Đào, Tuấn Trần, Trấn Thành",
                PosterUrl = "https://upload.wikimedia.org/wikipedia/vi/5/56/Poster_phim_Mai.jpg",
                Rating = 8.2m, AgeRating = "C16", Status = "NowShowing"
            },
            new() {
                Title = "Kẻ Trộm Mặt Trăng 4",
                OriginalTitle = "Despicable Me 4",
                Description = "Gru và những minion trở lại với cuộc phiêu lưu mới đầy tiếng cười, màu sắc và những bất ngờ thú vị cho cả gia đình.",
                Duration = 95, ReleaseDate = new DateOnly(2024, 7, 3),
                Language = "Tiếng Việt", Country = "Mỹ", Director = "Chris Renaud",
                Cast = "Steve Carell (lồng tiếng)",
                PosterUrl = "https://images.unsplash.com/photo-1585951237318-9ea5e175b891?w=400",
                Rating = 7.8m, AgeRating = "P", Status = "NowShowing"
            },
            new() {
                Title = "Quỷ Nhập Tràng",
                OriginalTitle = "The Evil Spirit",
                Description = "Một ngôi làng bị ám bởi thế lực bóng tối khi những ngôi mộ cổ bị xâm phạm. Bộ phim kinh dị Việt Nam đầy ám ảnh.",
                Duration = 105, ReleaseDate = new DateOnly(2024, 10, 25),
                Language = "Tiếng Việt", Country = "Việt Nam", Director = "Lương Đình Dũng",
                Cast = "Kaity Nguyễn, Hồng Đào, Isaac",
                PosterUrl = "https://images.unsplash.com/photo-1509347528160-9a9e33742cdb?w=400",
                Rating = 7.0m, AgeRating = "C16", Status = "NowShowing"
            },
            new() {
                Title = "Inside Out 2",
                OriginalTitle = "Inside Out 2",
                Description = "Riley bước vào tuổi thiếu niên, cảm xúc mới xuất hiện và thách thức sự cân bằng trong tâm trí cô bé.",
                Duration = 100, ReleaseDate = new DateOnly(2024, 6, 14),
                Language = "Tiếng Việt", Country = "Mỹ", Director = "Kelsey Mann",
                Cast = "Amy Poehler, Maya Hawke (lồng tiếng)",
                PosterUrl = "https://images.unsplash.com/photo-1560800452-f2d475982b96?w=400",
                Rating = 8.1m, AgeRating = "P", Status = "NowShowing"
            },
            new() {
                Title = "Deadpool & Wolverine",
                OriginalTitle = "Deadpool & Wolverine",
                Description = "Deadpool và Wolverine bắt tay nhau trong cuộc phiêu lưu đa vũ trụ đầy hài hước và hành động bùng nổ.",
                Duration = 128, ReleaseDate = new DateOnly(2024, 7, 26),
                Language = "Tiếng Anh", Country = "Mỹ", Director = "Shawn Levy",
                Cast = "Ryan Reynolds, Hugh Jackman",
                PosterUrl = "https://images.unsplash.com/photo-1531259683007-016a7b628fc3?w=400",
                Rating = 8.3m, AgeRating = "C18", Status = "NowShowing"
            },
            new() {
                Title = "Transformers One",
                OriginalTitle = "Transformers One",
                Description = "Khám phá nguồn gốc của cuộc chiến giữa Autobots và Decepticons qua câu chuyện về tình bạn và phản bội.",
                Duration = 104, ReleaseDate = new DateOnly(2025, 9, 20),
                Language = "Tiếng Việt", Country = "Mỹ", Director = "Josh Cooley",
                Cast = "Chris Hemsworth, Brian Tyree Henry (lồng tiếng)",
                PosterUrl = "https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=400",
                Rating = 0m, AgeRating = "C13", Status = "ComingSoon"
            },
        };
        context.Movies.AddRange(movies);
        context.SaveChanges();

        // ===== MOVIE GENRES =====
        var movieGenres = new List<MovieGenre>
        {
            new() { MovieId = movies[0].Id, GenreId = genres[7].Id }, // Đất Rừng - Phiêu lưu
            new() { MovieId = movies[0].Id, GenreId = genres[6].Id }, // Đất Rừng - Tâm lý
            new() { MovieId = movies[1].Id, GenreId = genres[0].Id }, // Avengers - Hành động
            new() { MovieId = movies[1].Id, GenreId = genres[5].Id }, // Avengers - Khoa học
            new() { MovieId = movies[2].Id, GenreId = genres[3].Id }, // Mai - Tình cảm
            new() { MovieId = movies[2].Id, GenreId = genres[6].Id }, // Mai - Tâm lý
            new() { MovieId = movies[3].Id, GenreId = genres[4].Id }, // Despicable - Hoạt hình
            new() { MovieId = movies[3].Id, GenreId = genres[1].Id }, // Despicable - Hài
            new() { MovieId = movies[4].Id, GenreId = genres[2].Id }, // Quỷ - Kinh dị
            new() { MovieId = movies[5].Id, GenreId = genres[4].Id }, // Inside Out - Hoạt hình
            new() { MovieId = movies[5].Id, GenreId = genres[6].Id }, // Inside Out - Tâm lý
            new() { MovieId = movies[6].Id, GenreId = genres[0].Id }, // Deadpool - Hành động
            new() { MovieId = movies[6].Id, GenreId = genres[1].Id }, // Deadpool - Hài
            new() { MovieId = movies[7].Id, GenreId = genres[0].Id }, // Transformers - Hành động
            new() { MovieId = movies[7].Id, GenreId = genres[5].Id }, // Transformers - Khoa học
        };
        context.MovieGenres.AddRange(movieGenres);
        context.SaveChanges();

        // ===== CINEMAS =====
        var cinemas = new List<Cinema>
        {
            new() { Name = "Cinema Star Hà Nội", Address = "72 Nguyễn Trãi, Thanh Xuân", City = "Hà Nội", Phone = "024.7300.1234", Email = "hanoi@cinemastar.vn", IsActive = true },
            new() { Name = "Cinema Star TP.HCM", Address = "45 Lê Lợi, Quận 1", City = "TP. Hồ Chí Minh", Phone = "028.7300.5678", Email = "hcm@cinemastar.vn", IsActive = true },
            new() { Name = "Cinema Star Đà Nẵng", Address = "10 Nguyễn Văn Linh, Hải Châu", City = "Đà Nẵng", Phone = "0236.7300.9012", Email = "danang@cinemastar.vn", IsActive = true },
        };
        context.Cinemas.AddRange(cinemas);
        context.SaveChanges();

        // ===== ROOMS =====
        var rooms = new List<Room>
        {
            new() { CinemaId = cinemas[0].Id, Name = "Phòng 1 - Standard", TotalSeats = 80, RoomType = "Standard", IsActive = true },
            new() { CinemaId = cinemas[0].Id, Name = "Phòng 2 - VIP", TotalSeats = 50, RoomType = "VIP", IsActive = true },
            new() { CinemaId = cinemas[1].Id, Name = "Phòng 1 - Standard", TotalSeats = 80, RoomType = "Standard", IsActive = true },
            new() { CinemaId = cinemas[1].Id, Name = "Phòng 2 - IMAX", TotalSeats = 100, RoomType = "IMAX", IsActive = true },
            new() { CinemaId = cinemas[2].Id, Name = "Phòng 1 - Standard", TotalSeats = 60, RoomType = "Standard", IsActive = true },
            new() { CinemaId = cinemas[2].Id, Name = "Phòng 2 - 4DX", TotalSeats = 40, RoomType = "4DX", IsActive = true },
        };
        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        // ===== SEATS for Room 1 (Hà Nội Standard) - 8 rows x 10 seats =====
        var seats = new List<Seat>();
        var rows = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
        foreach (var room in rooms)
        {
            int numRows = room.RoomType == "IMAX" ? 10 : (room.RoomType == "4DX" ? 5 : 8);
            int numCols = room.RoomType == "IMAX" ? 10 : (room.RoomType == "4DX" ? 8 : 10);
            var rowLabels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 1; c <= numCols; c++)
                {
                    string seatType = "Regular";
                    if (room.RoomType == "VIP" || r >= numRows - 2) seatType = "VIP";
                    if (room.RoomType == "4DX") seatType = "VIP";

                    seats.Add(new Seat
                    {
                        RoomId = room.Id,
                        RowLabel = rowLabels[r],
                        SeatNumber = c,
                        SeatType = seatType,
                        IsActive = true
                    });
                }
            }
        }
        context.Seats.AddRange(seats);
        context.SaveChanges();

        // ===== SHOWTIMES =====
        var now = DateTime.Now;
        var showtimes = new List<Showtime>
        {
            // Đất Rừng - HN Standard
            new() { MovieId = movies[0].Id, RoomId = rooms[0].Id, StartTime = now.AddHours(2), EndTime = now.AddHours(4), SubType = "Vietsub", Format = "2D", BasePrice = 85000, Status = "Scheduled" },
            new() { MovieId = movies[0].Id, RoomId = rooms[0].Id, StartTime = now.AddHours(5), EndTime = now.AddHours(7), SubType = "Dubbed", Format = "2D", BasePrice = 85000, Status = "Scheduled" },
            // Mai - HN VIP
            new() { MovieId = movies[2].Id, RoomId = rooms[1].Id, StartTime = now.AddHours(1), EndTime = now.AddHours(3.5), SubType = "Vietsub", Format = "2D", BasePrice = 120000, Status = "Scheduled" },
            new() { MovieId = movies[2].Id, RoomId = rooms[1].Id, StartTime = now.AddHours(6), EndTime = now.AddHours(8.5), SubType = "Vietsub", Format = "2D", BasePrice = 120000, Status = "Scheduled" },
            // Deadpool - HCM Standard
            new() { MovieId = movies[6].Id, RoomId = rooms[2].Id, StartTime = now.AddHours(3), EndTime = now.AddHours(5.5), SubType = "Original", Format = "2D", BasePrice = 95000, Status = "Scheduled" },
            // Inside Out - HCM IMAX
            new() { MovieId = movies[5].Id, RoomId = rooms[3].Id, StartTime = now.AddHours(2), EndTime = now.AddHours(3.75), SubType = "Vietsub", Format = "IMAX", BasePrice = 160000, Status = "Scheduled" },
            // Kẻ Trộm MT - DN Standard
            new() { MovieId = movies[3].Id, RoomId = rooms[4].Id, StartTime = now.AddHours(1), EndTime = now.AddHours(2.75), SubType = "Vietsub", Format = "2D", BasePrice = 75000, Status = "Scheduled" },
            // Quỷ Nhập - DN 4DX
            new() { MovieId = movies[4].Id, RoomId = rooms[5].Id, StartTime = now.AddHours(4), EndTime = now.AddHours(5.75), SubType = "Vietsub", Format = "4DX", BasePrice = 200000, Status = "Scheduled" },
            // Tomorrow showtimes
            new() { MovieId = movies[0].Id, RoomId = rooms[2].Id, StartTime = now.AddDays(1).AddHours(9), EndTime = now.AddDays(1).AddHours(11), SubType = "Vietsub", Format = "2D", BasePrice = 85000, Status = "Scheduled" },
            new() { MovieId = movies[6].Id, RoomId = rooms[3].Id, StartTime = now.AddDays(1).AddHours(14), EndTime = now.AddDays(1).AddHours(16.5), SubType = "Vietsub", Format = "IMAX", BasePrice = 160000, Status = "Scheduled" },
        };
        context.Showtimes.AddRange(showtimes);
        context.SaveChanges();

        // ===== BOOKINGS & PAYMENTS =====
        var bookingCode1 = "BK" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd") + "001";
        var bookingCode2 = "BK" + DateTime.Now.AddDays(-7).ToString("yyyyMMdd") + "002";
        var bookingCode3 = "BK" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd") + "003";

        // Get seats for first showtime
        var showtime1Seats = context.Seats
            .Where(s => s.RoomId == rooms[0].Id && s.IsActive)
            .OrderBy(s => s.RowLabel).ThenBy(s => s.SeatNumber)
            .Take(10).ToList();

        var booking1 = new Booking
        {
            BookingCode = bookingCode1,
            UserId = users[1].Id,
            ShowtimeId = showtimes[0].Id,
            TotalAmount = 170000,
            Status = "Confirmed",
            CreatedAt = DateTime.Now.AddDays(-10)
        };
        context.Bookings.Add(booking1);
        context.SaveChanges();

        if (showtime1Seats.Count >= 2)
        {
            context.BookingSeats.AddRange(
                new BookingSeat { BookingId = booking1.Id, SeatId = showtime1Seats[0].Id, Price = 85000 },
                new BookingSeat { BookingId = booking1.Id, SeatId = showtime1Seats[1].Id, Price = 85000 }
            );
        }
        context.Payments.Add(new Payment
        {
            BookingId = booking1.Id,
            Method = "Mock",
            Amount = 170000,
            TransactionCode = "TXN" + bookingCode1,
            Status = "Success",
            PaidAt = DateTime.Now.AddDays(-10).AddMinutes(5)
        });
        context.SaveChanges();

        var booking2 = new Booking
        {
            BookingCode = bookingCode2,
            UserId = users[2].Id,
            ShowtimeId = showtimes[2].Id,
            TotalAmount = 360000,
            Status = "Confirmed",
            CreatedAt = DateTime.Now.AddDays(-7)
        };
        context.Bookings.Add(booking2);
        context.SaveChanges();

        var showtime3Seats = context.Seats
            .Where(s => s.RoomId == rooms[1].Id && s.IsActive)
            .OrderBy(s => s.RowLabel).ThenBy(s => s.SeatNumber)
            .Take(10).ToList();

        if (showtime3Seats.Count >= 3)
        {
            context.BookingSeats.AddRange(
                new BookingSeat { BookingId = booking2.Id, SeatId = showtime3Seats[0].Id, Price = 120000 },
                new BookingSeat { BookingId = booking2.Id, SeatId = showtime3Seats[1].Id, Price = 120000 },
                new BookingSeat { BookingId = booking2.Id, SeatId = showtime3Seats[2].Id, Price = 120000 }
            );
        }
        context.Payments.Add(new Payment
        {
            BookingId = booking2.Id,
            Method = "Mock",
            Amount = 360000,
            TransactionCode = "TXN" + bookingCode2,
            Status = "Success",
            PaidAt = DateTime.Now.AddDays(-7).AddMinutes(3)
        });
        context.SaveChanges();

        var booking3 = new Booking
        {
            BookingCode = bookingCode3,
            UserId = users[1].Id,
            ShowtimeId = showtimes[4].Id,
            TotalAmount = 95000,
            Status = "Pending",
            CreatedAt = DateTime.Now.AddDays(-3)
        };
        context.Bookings.Add(booking3);
        context.SaveChanges();

        var showtime5Seats = context.Seats
            .Where(s => s.RoomId == rooms[2].Id && s.IsActive)
            .OrderBy(s => s.RowLabel).ThenBy(s => s.SeatNumber)
            .Take(5).ToList();

        if (showtime5Seats.Count >= 1)
        {
            context.BookingSeats.Add(
                new BookingSeat { BookingId = booking3.Id, SeatId = showtime5Seats[0].Id, Price = 95000 }
            );
        }
        context.Payments.Add(new Payment
        {
            BookingId = booking3.Id,
            Method = "Mock",
            Amount = 95000,
            Status = "Pending"
        });
        context.SaveChanges();

        // ===== BANNERS =====
        var banners = new List<Banner>
        {
            new() { Title = "Đất Rừng Phương Nam - Đang chiếu", ImageUrl = "https://images.unsplash.com/photo-1518676590629-3dcbd9c5a5c9?w=1200", LinkUrl = "/Movie/Details/1", IsActive = true, SortOrder = 1 },
            new() { Title = "Ưu đãi cuối tuần - Giảm 20%", ImageUrl = "https://images.unsplash.com/photo-1485846234645-a62644f84728?w=1200", IsActive = true, SortOrder = 2 },
            new() { Title = "Combo bắp nước đặc biệt", ImageUrl = "https://images.unsplash.com/photo-1535016120720-40c646be5580?w=1200", IsActive = true, SortOrder = 3 },
        };
        context.Banners.AddRange(banners);
        context.SaveChanges();
    }
}
