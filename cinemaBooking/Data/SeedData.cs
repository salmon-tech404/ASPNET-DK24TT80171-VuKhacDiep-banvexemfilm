using cinemaBooking.Models.Domain;

namespace cinemaBooking.Data;

public static class SeedData
{
    public static void Initialize(CinemaDbContext context)
    {
        if (context.NguoiDung.Any()) return;

        // ===== USERS (NguoiDung) =====
        var users = new List<NguoiDung>
        {
            new() { HoTen = "Quản Trị Viên", Email = "admin@cinema.com", MatKhau = "Admin@123", DienThoai = "0900000001", VaiTro = "Admin", TrangThai = true, NgayTao = DateTime.Now.AddDays(-90) },
            new() { HoTen = "Vũ Khắc Diệp", Email = "diep@cinema.com", MatKhau = "Customer@123", DienThoai = "0900000002", VaiTro = "Customer", TrangThai = true, NgayTao = DateTime.Now.AddDays(-60) },
            new() { HoTen = "Nguyễn Thị Lan", Email = "lan@cinema.com", MatKhau = "Customer@123", DienThoai = "0900000003", VaiTro = "Customer", TrangThai = true, NgayTao = DateTime.Now.AddDays(-50) },
            new() { HoTen = "Trần Văn Hùng", Email = "hung@cinema.com", MatKhau = "Customer@123", DienThoai = "0900000004", VaiTro = "Customer", TrangThai = true, NgayTao = DateTime.Now.AddDays(-45) },
            new() { HoTen = "Phạm Minh Tuấn", Email = "tuan@cinema.com", MatKhau = "Customer@123", DienThoai = "0900000005", VaiTro = "Customer", TrangThai = true, NgayTao = DateTime.Now.AddDays(-30) },
            new() { HoTen = "Lê Thị Mai", Email = "mai@cinema.com", MatKhau = "Customer@123", DienThoai = "0900000006", VaiTro = "Customer", TrangThai = false, NgayTao = DateTime.Now.AddDays(-20) },
        };
        context.NguoiDung.AddRange(users);
        context.SaveChanges();

        // ===== GENRES (TheLoai) =====
        var genres = new List<TheLoai>
        {
            new() { TenTheLoai = "Hành động" },
            new() { TenTheLoai = "Hài hước" },
            new() { TenTheLoai = "Kinh dị" },
            new() { TenTheLoai = "Tình cảm" },
            new() { TenTheLoai = "Hoạt hình" },
            new() { TenTheLoai = "Khoa học viễn tưởng" },
            new() { TenTheLoai = "Tâm lý" },
            new() { TenTheLoai = "Phiêu lưu" },
        };
        context.TheLoai.AddRange(genres);
        context.SaveChanges();

        // ===== MOVIES (Phim) =====
        var movies = new List<Phim>
        {
            new() {
                TenPhim = "Đất Rừng Phương Nam",
                TenGoc = "Southern Forest",
                MoTa = "Bộ phim mang đến cho khán giả hành trình khám phá vùng đất phương Nam hoang sơ, hùng vĩ với câu chuyện đầy cảm xúc về tình đất, tình người.",
                ThoiLuong = 115, NgayChieu = new DateOnly(2024, 10, 1), NgayKetThuc = new DateOnly(2024, 12, 31),
                NgonNgu = "Tiếng Việt", QuocGia = "Việt Nam", DaoDien = "Nguyễn Quang Dũng",
                DienVien = "Hứa Vĩ Văn, Tuấn Trần, Tiến Luật",
                AnhPoster = "https://upload.wikimedia.org/wikipedia/vi/4/4e/%C4%90%E1%BA%A5t_r%E1%BB%ABng_ph%C6%B0%C6%A1ng_Nam.jpg",
                DiemDanhGia = 7.5m, DoTuoiQuyDinh = "C13", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Avengers: Secret Wars",
                TenGoc = "Avengers: Secret Wars",
                MoTa = "Các siêu anh hùng của Vũ trụ Marvel tập hợp một lần nữa để đối mặt với mối đe dọa lớn nhất từ trước đến nay.",
                ThoiLuong = 165, NgayChieu = new DateOnly(2025, 5, 2),
                NgonNgu = "Tiếng Anh", QuocGia = "Mỹ", DaoDien = "Russo Brothers",
                DienVien = "Robert Downey Jr., Chris Evans, Scarlett Johansson",
                AnhPoster = "https://images.unsplash.com/photo-1612036782180-6f0b6cd846fe?w=400",
                DiemDanhGia = 8.5m, DoTuoiQuyDinh = "C13", TrangThaiChieu = "ComingSoon"
            },
            new() {
                TenPhim = "Mai",
                TenGoc = "Mai",
                MoTa = "Câu chuyện tình yêu đầy cảm xúc giữa hai con người có hoàn cảnh khác nhau, vượt qua những rào cản của cuộc sống để tìm đến hạnh phúc.",
                ThoiLuong = 130, NgayChieu = new DateOnly(2024, 2, 10), NgayKetThuc = new DateOnly(2024, 4, 30),
                NgonNgu = "Tiếng Việt", QuocGia = "Việt Nam", DaoDien = "Trấn Thành",
                DienVien = "Phương Anh Đào, Tuấn Trần, Trấn Thành",
                AnhPoster = "https://upload.wikimedia.org/wikipedia/vi/5/56/Poster_phim_Mai.jpg",
                DiemDanhGia = 8.2m, DoTuoiQuyDinh = "C16", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Kẻ Trộm Mặt Trăng 4",
                TenGoc = "Despicable Me 4",
                MoTa = "Gru và những minion trở lại với cuộc phiêu lưu mới đầy tiếng cười, màu sắc và những bất ngờ thú vị cho cả gia đình.",
                ThoiLuong = 95, NgayChieu = new DateOnly(2024, 7, 3),
                NgonNgu = "Tiếng Việt", QuocGia = "Mỹ", DaoDien = "Chris Renaud",
                DienVien = "Steve Carell (lồng tiếng)",
                AnhPoster = "https://images.unsplash.com/photo-1585951237318-9ea5e175b891?w=400",
                DiemDanhGia = 7.8m, DoTuoiQuyDinh = "P", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Quỷ Nhập Tràng",
                TenGoc = "The Evil Spirit",
                MoTa = "Một ngôi làng bị ám bởi thế lực bóng tối khi những ngôi mộ cổ bị xâm phạm. Bộ phim kinh dị Việt Nam đầy ám ảnh.",
                ThoiLuong = 105, NgayChieu = new DateOnly(2024, 10, 25),
                NgonNgu = "Tiếng Việt", QuocGia = "Việt Nam", DaoDien = "Lương Đình Dũng",
                DienVien = "Kaity Nguyễn, Hồng Đào, Isaac",
                AnhPoster = "https://images.unsplash.com/photo-1509347528160-9a9e33742cdb?w=400",
                DiemDanhGia = 7.0m, DoTuoiQuyDinh = "C16", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Inside Out 2",
                TenGoc = "Inside Out 2",
                MoTa = "Riley bước vào tuổi thiếu niên, cảm xúc mới xuất hiện và thách thức sự cân bằng trong tâm trí cô bé.",
                ThoiLuong = 100, NgayChieu = new DateOnly(2024, 6, 14),
                NgonNgu = "Tiếng Việt", QuocGia = "Mỹ", DaoDien = "Kelsey Mann",
                DienVien = "Amy Poehler, Maya Hawke (lồng tiếng)",
                AnhPoster = "https://images.unsplash.com/photo-1560800452-f2d475982b96?w=400",
                DiemDanhGia = 8.1m, DoTuoiQuyDinh = "P", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Deadpool & Wolverine",
                TenGoc = "Deadpool & Wolverine",
                MoTa = "Deadpool và Wolverine bắt tay nhau trong cuộc phiêu lưu đa vũ trụ đầy hài hước và hành động bùng nổ.",
                ThoiLuong = 128, NgayChieu = new DateOnly(2024, 7, 26),
                NgonNgu = "Tiếng Anh", QuocGia = "Mỹ", DaoDien = "Shawn Levy",
                DienVien = "Ryan Reynolds, Hugh Jackman",
                AnhPoster = "https://images.unsplash.com/photo-1531259683007-016a7b628fc3?w=400",
                DiemDanhGia = 8.3m, DoTuoiQuyDinh = "C18", TrangThaiChieu = "NowShowing"
            },
            new() {
                TenPhim = "Transformers One",
                TenGoc = "Transformers One",
                MoTa = "Khám phá nguồn gốc của cuộc chiến giữa Autobots và Decepticons qua câu chuyện về tình bạn và phản bội.",
                ThoiLuong = 104, NgayChieu = new DateOnly(2025, 9, 20),
                NgonNgu = "Tiếng Việt", QuocGia = "Mỹ", DaoDien = "Josh Cooley",
                DienVien = "Chris Hemsworth, Brian Tyree Henry (lồng tiếng)",
                AnhPoster = "https://images.unsplash.com/photo-1574375927938-d5a98e8ffe85?w=400",
                DiemDanhGia = 0m, DoTuoiQuyDinh = "C13", TrangThaiChieu = "ComingSoon"
            },
        };
        context.Phim.AddRange(movies);
        context.SaveChanges();

        // ===== MOVIE GENRES (TheLoaiPhim) =====
        var movieGenres = new List<TheLoaiPhim>
        {
            new() { MaPhim = movies[0].Id, MaTheLoai = genres[7].Id }, // Đất Rừng - Phiêu lưu
            new() { MaPhim = movies[0].Id, MaTheLoai = genres[6].Id }, // Đất Rừng - Tâm lý
            new() { MaPhim = movies[1].Id, MaTheLoai = genres[0].Id }, // Avengers - Hành động
            new() { MaPhim = movies[1].Id, MaTheLoai = genres[5].Id }, // Avengers - Khoa học
            new() { MaPhim = movies[2].Id, MaTheLoai = genres[3].Id }, // Mai - Tình cảm
            new() { MaPhim = movies[2].Id, MaTheLoai = genres[6].Id }, // Mai - Tâm lý
            new() { MaPhim = movies[3].Id, MaTheLoai = genres[4].Id }, // Despicable - Hoạt hình
            new() { MaPhim = movies[3].Id, MaTheLoai = genres[1].Id }, // Despicable - Hài
            new() { MaPhim = movies[4].Id, MaTheLoai = genres[2].Id }, // Quỷ - Kinh dị
            new() { MaPhim = movies[5].Id, MaTheLoai = genres[4].Id }, // Inside Out - Hoạt hình
            new() { MaPhim = movies[5].Id, MaTheLoai = genres[6].Id }, // Inside Out - Tâm lý
            new() { MaPhim = movies[6].Id, MaTheLoai = genres[0].Id }, // Deadpool - Hành động
            new() { MaPhim = movies[6].Id, MaTheLoai = genres[1].Id }, // Deadpool - Hài
            new() { MaPhim = movies[7].Id, MaTheLoai = genres[0].Id }, // Transformers - Hành động
            new() { MaPhim = movies[7].Id, MaTheLoai = genres[5].Id }, // Transformers - Khoa học
        };
        context.TheLoaiPhim.AddRange(movieGenres);
        context.SaveChanges();

        // ===== CINEMAS (RapChieu) =====
        var cinemas = new List<RapChieu>
        {
            new() { TenRap = "Cinema Star Hà Nội", DiaChi = "72 Nguyễn Trãi, Thanh Xuân", ThanhPho = "Hà Nội", DienThoai = "024.7300.1234", Email = "hanoi@cinemastar.vn", TrangThai = true },
            new() { TenRap = "Cinema Star TP.HCM", DiaChi = "45 Lê Lợi, Quận 1", ThanhPho = "TP. Hồ Chí Minh", DienThoai = "028.7300.5678", Email = "hcm@cinemastar.vn", TrangThai = true },
            new() { TenRap = "Cinema Star Đà Nẵng", DiaChi = "10 Nguyễn Văn Linh, Hải Châu", ThanhPho = "Đà Nẵng", DienThoai = "0236.7300.9012", Email = "danang@cinemastar.vn", TrangThai = true },
        };
        context.RapChieu.AddRange(cinemas);
        context.SaveChanges();

        // ===== ROOMS (PhongChieu) =====
        var rooms = new List<PhongChieu>
        {
            new() { MaRap = cinemas[0].Id, TenPhong = "Phòng 1 - Standard", TongSoGhe = 80, LoaiPhong = "Standard", TrangThai = true },
            new() { MaRap = cinemas[0].Id, TenPhong = "Phòng 2 - VIP", TongSoGhe = 50, LoaiPhong = "VIP", TrangThai = true },
            new() { MaRap = cinemas[1].Id, TenPhong = "Phòng 1 - Standard", TongSoGhe = 80, LoaiPhong = "Standard", TrangThai = true },
            new() { MaRap = cinemas[1].Id, TenPhong = "Phòng 2 - IMAX", TongSoGhe = 100, LoaiPhong = "IMAX", TrangThai = true },
            new() { MaRap = cinemas[2].Id, TenPhong = "Phòng 1 - Standard", TongSoGhe = 60, LoaiPhong = "Standard", TrangThai = true },
            new() { MaRap = cinemas[2].Id, TenPhong = "Phòng 2 - 4DX", TongSoGhe = 40, LoaiPhong = "4DX", TrangThai = true },
        };
        context.PhongChieu.AddRange(rooms);
        context.SaveChanges();

        // ===== SEATS (GheNgoi) =====
        var seats = new List<GheNgoi>();
        foreach (var room in rooms)
        {
            int numRows = room.LoaiPhong == "IMAX" ? 10 : (room.LoaiPhong == "4DX" ? 5 : 8);
            int numCols = room.LoaiPhong == "IMAX" ? 10 : (room.LoaiPhong == "4DX" ? 8 : 10);
            var rowLabels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 1; c <= numCols; c++)
                {
                    string seatType = "Regular";
                    if (room.LoaiPhong == "VIP" || r >= numRows - 2) seatType = "VIP";
                    if (room.LoaiPhong == "4DX") seatType = "VIP";

                    seats.Add(new GheNgoi
                    {
                        MaPhong = room.Id,
                        HangGhe = rowLabels[r],
                        SoGhe = c,
                        LoaiGhe = seatType,
                        TrangThai = true
                    });
                }
            }
        }
        context.GheNgoi.AddRange(seats);
        context.SaveChanges();

        // ===== SHOWTIMES (SuatChieu) =====
        var now = DateTime.Now;
        var showtimes = new List<SuatChieu>
        {
            // Đất Rừng - HN Standard
            new() { MaPhim = movies[0].Id, MaPhong = rooms[0].Id, GioBatDau = now.AddHours(2), GioKetThuc = now.AddHours(4), PhongDich = "Vietsub", DinhDang = "2D", GiaVeCoBan = 85000, TrangThaiSuat = "Scheduled" },
            new() { MaPhim = movies[0].Id, MaPhong = rooms[0].Id, GioBatDau = now.AddHours(5), GioKetThuc = now.AddHours(7), PhongDich = "Dubbed", DinhDang = "2D", GiaVeCoBan = 85000, TrangThaiSuat = "Scheduled" },
            // Mai - HN VIP
            new() { MaPhim = movies[2].Id, MaPhong = rooms[1].Id, GioBatDau = now.AddHours(1), GioKetThuc = now.AddHours(3.5), PhongDich = "Vietsub", DinhDang = "2D", GiaVeCoBan = 120000, TrangThaiSuat = "Scheduled" },
            new() { MaPhim = movies[2].Id, MaPhong = rooms[1].Id, GioBatDau = now.AddHours(6), GioKetThuc = now.AddHours(8.5), PhongDich = "Vietsub", DinhDang = "2D", GiaVeCoBan = 120000, TrangThaiSuat = "Scheduled" },
            // Deadpool - HCM Standard
            new() { MaPhim = movies[6].Id, MaPhong = rooms[2].Id, GioBatDau = now.AddHours(3), GioKetThuc = now.AddHours(5.5), PhongDich = "Original", DinhDang = "2D", GiaVeCoBan = 95000, TrangThaiSuat = "Scheduled" },
            // Inside Out - HCM IMAX
            new() { MaPhim = movies[5].Id, MaPhong = rooms[3].Id, GioBatDau = now.AddHours(2), GioKetThuc = now.AddHours(3.75), PhongDich = "Vietsub", DinhDang = "IMAX", GiaVeCoBan = 160000, TrangThaiSuat = "Scheduled" },
            // Kẻ Trộm MT - DN Standard
            new() { MaPhim = movies[3].Id, MaPhong = rooms[4].Id, GioBatDau = now.AddHours(1), GioKetThuc = now.AddHours(2.75), PhongDich = "Vietsub", DinhDang = "2D", GiaVeCoBan = 75000, TrangThaiSuat = "Scheduled" },
            // Quỷ Nhập - DN 4DX
            new() { MaPhim = movies[4].Id, MaPhong = rooms[5].Id, GioBatDau = now.AddHours(4), GioKetThuc = now.AddHours(5.75), PhongDich = "Vietsub", DinhDang = "4DX", GiaVeCoBan = 200000, TrangThaiSuat = "Scheduled" },
            // Tomorrow showtimes
            new() { MaPhim = movies[0].Id, MaPhong = rooms[2].Id, GioBatDau = now.AddDays(1).AddHours(9), GioKetThuc = now.AddDays(1).AddHours(11), PhongDich = "Vietsub", DinhDang = "2D", GiaVeCoBan = 85000, TrangThaiSuat = "Scheduled" },
            new() { MaPhim = movies[6].Id, MaPhong = rooms[3].Id, GioBatDau = now.AddDays(1).AddHours(14), GioKetThuc = now.AddDays(1).AddHours(16.5), PhongDich = "Vietsub", DinhDang = "IMAX", GiaVeCoBan = 160000, TrangThaiSuat = "Scheduled" },
        };
        context.SuatChieu.AddRange(showtimes);
        context.SaveChanges();

        // ===== BOOKINGS & PAYMENTS (DatVe & ThanhToan) =====
        var bookingCode1 = "BK" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd") + "001";
        var bookingCode2 = "BK" + DateTime.Now.AddDays(-7).ToString("yyyyMMdd") + "002";
        var bookingCode3 = "BK" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd") + "003";

        // Get seats for first showtime
        var showtime1Seats = context.GheNgoi
            .Where(s => s.MaPhong == rooms[0].Id && s.TrangThai)
            .OrderBy(s => s.HangGhe).ThenBy(s => s.SoGhe)
            .Take(10).ToList();

        var booking1 = new DatVe
        {
            MaGiaoDich = bookingCode1,
            MaNguoiDung = users[1].Id,
            MaSuatChieu = showtimes[0].Id,
            TongTien = 170000,
            TrangThaiDat = "Confirmed",
            NgayDat = DateTime.Now.AddDays(-10)
        };
        context.DatVe.Add(booking1);
        context.SaveChanges();

        if (showtime1Seats.Count >= 2)
        {
            context.ChiTietGheDat.AddRange(
                new ChiTietGheDat { MaDatVe = booking1.Id, MaGhe = showtime1Seats[0].Id, GiaVeThucTe = 85000 },
                new ChiTietGheDat { MaDatVe = booking1.Id, MaGhe = showtime1Seats[1].Id, GiaVeThucTe = 85000 }
            );
        }
        context.ThanhToan.Add(new ThanhToan
        {
            MaDatVe = booking1.Id,
            PhuongThuc = "Mock",
            SoTien = 170000,
            MaGiaoDichNganHang = "TXN" + bookingCode1,
            TrangThaiThanhToan = "Success",
            NgayThanhToan = DateTime.Now.AddDays(-10).AddMinutes(5)
        });
        context.SaveChanges();

        var booking2 = new DatVe
        {
            MaGiaoDich = bookingCode2,
            MaNguoiDung = users[2].Id,
            MaSuatChieu = showtimes[2].Id,
            TongTien = 360000,
            TrangThaiDat = "Confirmed",
            NgayDat = DateTime.Now.AddDays(-7)
        };
        context.DatVe.Add(booking2);
        context.SaveChanges();

        var showtime3Seats = context.GheNgoi
            .Where(s => s.MaPhong == rooms[1].Id && s.TrangThai)
            .OrderBy(s => s.HangGhe).ThenBy(s => s.SoGhe)
            .Take(10).ToList();

        if (showtime3Seats.Count >= 3)
        {
            context.ChiTietGheDat.AddRange(
                new ChiTietGheDat { MaDatVe = booking2.Id, MaGhe = showtime3Seats[0].Id, GiaVeThucTe = 120000 },
                new ChiTietGheDat { MaDatVe = booking2.Id, MaGhe = showtime3Seats[1].Id, GiaVeThucTe = 120000 },
                new ChiTietGheDat { MaDatVe = booking2.Id, MaGhe = showtime3Seats[2].Id, GiaVeThucTe = 120000 }
            );
        }
        context.ThanhToan.Add(new ThanhToan
        {
            MaDatVe = booking2.Id,
            PhuongThuc = "Mock",
            SoTien = 360000,
            MaGiaoDichNganHang = "TXN" + bookingCode2,
            TrangThaiThanhToan = "Success",
            NgayThanhToan = DateTime.Now.AddDays(-7).AddMinutes(3)
        });
        context.SaveChanges();

        var booking3 = new DatVe
        {
            MaGiaoDich = bookingCode3,
            MaNguoiDung = users[1].Id,
            MaSuatChieu = showtimes[4].Id,
            TongTien = 95000,
            TrangThaiDat = "Pending",
            NgayDat = DateTime.Now.AddDays(-3)
        };
        context.DatVe.Add(booking3);
        context.SaveChanges();

        var showtime5Seats = context.GheNgoi
            .Where(s => s.MaPhong == rooms[2].Id && s.TrangThai)
            .OrderBy(s => s.HangGhe).ThenBy(s => s.SoGhe)
            .Take(5).ToList();

        if (showtime5Seats.Count >= 1)
        {
            context.ChiTietGheDat.Add(
                new ChiTietGheDat { MaDatVe = booking3.Id, MaGhe = showtime5Seats[0].Id, GiaVeThucTe = 95000 }
            );
        }
        context.ThanhToan.Add(new ThanhToan
        {
            MaDatVe = booking3.Id,
            PhuongThuc = "Mock",
            SoTien = 95000,
            TrangThaiThanhToan = "Pending"
        });
        context.SaveChanges();

        // ===== BANNERS (BannerQuangCao) =====
        var banners = new List<BannerQuangCao>
        {
            new() { TieuDe = "Đất Rừng Phương Nam - Đang chiếu", DuongDanAnh = "https://images.unsplash.com/photo-1518676590629-3dcbd9c5a5c9?w=1200", DuongDanLienKet = "/Movie/Details/1", TrangThai = true, ThuTuHienThi = 1 },
            new() { TieuDe = "Ưu đãi cuối tuần - Giảm 20%", DuongDanAnh = "https://images.unsplash.com/photo-1485846234645-a62644f84728?w=1200", TrangThai = true, ThuTuHienThi = 2 },
            new() { TieuDe = "Combo bắp nước đặc biệt", DuongDanAnh = "https://images.unsplash.com/photo-1535016120720-40c646be5580?w=1200", TrangThai = true, ThuTuHienThi = 3 },
        };
        context.BannerQuangCao.AddRange(banners);
        context.SaveChanges();
    }
}
