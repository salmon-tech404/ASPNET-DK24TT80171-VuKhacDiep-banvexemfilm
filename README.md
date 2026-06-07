# Cinema Star – Hệ Thống Đặt Vé Xem Phim

Dự án ASP.NET Core MVC quản lý và đặt vé xem phim trực tuyến.
Tên sinh viên: **Vũ Khắc Diệp** | Mã đồ án: **DK24TT80171**

---

## 1. Tổng quan

Cinema Star là ứng dụng web cho phép người dùng duyệt danh sách phim đang chiếu / sắp chiếu, chọn suất chiếu, chọn ghế trực quan, và đặt vé trực tuyến. Hệ thống có phân quyền Admin và Khách hàng.

**Tính năng chính:**
- Khách vãng lai: xem phim, xem lịch chiếu, đăng ký tài khoản
- Khách hàng: đặt vé, chọn ghế theo sơ đồ, thanh toán (mock), xem lịch sử vé, hủy vé
- Admin: quản lý phim (CRUD), quản lý suất chiếu (CRUD), quản lý đơn đặt vé, quản lý người dùng, xem dashboard thống kê, xem rạp chiếu

---

## 2. Stack công nghệ

| Thành phần | Công nghệ |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| Ngôn ngữ | C# 12 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server (LocalDB mặc định) |
| Authentication | Cookie Authentication |
| Mật khẩu | BCrypt.Net-Next |
| UI | Bootstrap 5.3 + Font Awesome 6.4 (CDN) |
| Template engine | Razor Views (.cshtml) |

---

## 3. Cấu trúc thư mục

```
ASPNET-DK24TT80171-VuKhacDiep-banvexemfilm/
├── cinemaBooking/
│   ├── Controllers/
│   │   ├── AccountController.cs       # Đăng nhập, đăng ký, hồ sơ
│   │   ├── AdminController.cs         # Dashboard, quản lý phim/suất/đơn/user
│   │   ├── BookingController.cs       # Chọn ghế, tạo vé, thanh toán, lịch sử
│   │   ├── HomeController.cs          # Trang chủ
│   │   └── MovieController.cs         # Danh sách & chi tiết phim
│   ├── Data/
│   │   ├── CinemaDbContext.cs         # EF Core DbContext
│   │   └── SeedData.cs                # Dữ liệu mẫu khởi tạo
│   ├── Models/
│   │   ├── Domain/                    # 12 Entity models
│   │   │   ├── User.cs
│   │   │   ├── Movie.cs / Genre.cs / MovieGenre.cs
│   │   │   ├── Cinema.cs / Room.cs / Seat.cs
│   │   │   ├── Showtime.cs
│   │   │   ├── Booking.cs / BookingSeat.cs
│   │   │   ├── Payment.cs
│   │   │   └── Banner.cs
│   │   └── ViewModels/                # 5 ViewModel files
│   │       ├── AccountViewModels.cs
│   │       ├── MovieViewModels.cs
│   │       ├── BookingViewModels.cs
│   │       ├── ShowtimeViewModels.cs
│   │       └── DashboardViewModels.cs
│   ├── Services/
│   │   ├── Interfaces/                # 5 interface files
│   │   │   ├── IUserService.cs
│   │   │   ├── IMovieService.cs
│   │   │   ├── ICinemaService.cs
│   │   │   ├── IShowtimeService.cs
│   │   │   └── IBookingService.cs
│   │   ├── UserService.cs
│   │   ├── MovieService.cs
│   │   ├── CinemaService.cs
│   │   ├── ShowtimeService.cs
│   │   └── BookingService.cs
│   ├── Views/
│   │   ├── Account/  (Login, Register, Profile, ChangePassword, AccessDenied)
│   │   ├── Admin/    (Dashboard, Movies, Showtimes, Bookings, Users, Cinemas)
│   │   ├── Booking/  (SeatSelection, Confirmation, History)
│   │   ├── Home/     (Index)
│   │   ├── Movie/    (Index, Details, Create, Edit)
│   │   └── Shared/   (_Layout, Error, _ValidationScriptsPartial)
│   ├── wwwroot/css/site.css           # CSS tối màu cho giao diện cinema
│   ├── Program.cs
│   ├── appsettings.json
│   └── cinemaBooking.csproj
└── README.md
```

---

## 4. Cách chạy lần đầu

### Bước 1 – Clone / mở dự án
Mở folder `cinemaBooking` trong Visual Studio hoặc VS Code.

### Bước 2 – Cấu hình connection string

Mở `appsettings.json`:

**Dùng SQL Server LocalDB (mặc định):**
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CinemaBookingDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```

**Dùng SQL Server Express:**
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=CinemaBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
```

**Dùng SQL Server đặt tên riêng (ví dụ: `DIEP-PC\\MSSQLSERVER01`):**
```json
"DefaultConnection": "Server=DIEP-PC\\MSSQLSERVER01;Database=CinemaBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
```

### Bước 3 – Chạy dự án
```bash
cd cinemaBooking
dotnet run
```

Khi khởi động lần đầu, hệ thống tự động:
- Tạo database `CinemaBookingDB` (qua `EnsureCreated`)
- Seed dữ liệu mẫu (phim, rạp, phòng, ghế, suất chiếu, người dùng)

Truy cập: `https://localhost:7xxx` hoặc `http://localhost:5xxx` (xem cổng trong terminal)

---

## 5. Tài khoản demo

| Tên | Email | Mật khẩu | Vai trò |
|---|---|---|---|
| Quản Trị Viên | admin@cinema.com | Admin@123 | Admin |
| Vũ Khắc Diệp | diep@cinema.com | Customer@123 | Khách hàng |
| Nguyễn Thị Lan | lan@cinema.com | Customer@123 | Khách hàng |
| Trần Văn Hùng | hung@cinema.com | Customer@123 | Khách hàng |
| Phạm Minh Tuấn | tuan@cinema.com | Customer@123 | Khách hàng |

---

## 6. Lộ trình test end-to-end

### Flow 1 – Khách đặt vé
1. Vào trang chủ → xem phim đang chiếu
2. Click vào phim → trang **Chi tiết phim** với lịch chiếu
3. Chọn suất chiếu → trang **Chọn ghế**
4. Click chọn ghế trên sơ đồ → bấm **Tiếp tục**
5. Trang **Xác nhận** → bấm **Thanh toán ngay**
6. Vé được xác nhận, xem lại ở **Lịch sử đặt vé**

### Flow 2 – Đăng ký tài khoản
1. Vào `/Account/Register`
2. Điền đầy đủ thông tin → **Tạo tài khoản**
3. Chuyển sang trang đăng nhập → đăng nhập thành công

### Flow 3 – Admin quản lý phim
1. Đăng nhập `admin@cinema.com`
2. Vào `/Admin/Dashboard` → xem thống kê
3. Vào **Quản lý phim** → thêm / sửa / xóa phim
4. Vào **Suất chiếu** → thêm suất chiếu mới

### Flow 4 – Hủy vé
1. Vào **Lịch sử đặt vé** → chọn vé đã xác nhận
2. Bấm **Hủy** → xác nhận

---

## 7. Reset / Seed lại dữ liệu

Xóa database và chạy lại để có dữ liệu mẫu mới:

**Qua SQL Server Management Studio:**
```sql
DROP DATABASE CinemaBookingDB;
```

**Qua dotnet CLI:**
```bash
dotnet ef database drop --force
```

Sau đó chạy lại `dotnet run` – database và seed data sẽ được tạo lại.

---

## 8. Sơ đồ dữ liệu

```
Users ──────────────────────────── Bookings
  │                                    │
  │ (Role: Admin / Customer)           ├── BookingSeats ── Seats ── Rooms ── Cinemas
  │                                    │
  │                                    └── Payment
  │
Movies ──── MovieGenres ──── Genres
  │
  └── Showtimes ──── Rooms

Banners (độc lập)
```

| Bảng | Quan hệ chính |
|---|---|
| Users → Bookings | 1 user có nhiều bookings |
| Movies → MovieGenres → Genres | N:N qua MovieGenres |
| Cinemas → Rooms → Seats | 1 rạp nhiều phòng, 1 phòng nhiều ghế |
| Movies → Showtimes → Rooms | 1 phim nhiều suất chiếu tại nhiều phòng |
| Bookings → BookingSeats → Seats | N:N qua BookingSeats |
| Bookings → Payment | 1:1 |

---

## 9. Giá vé

Giá vé được tính động theo công thức:

```
Giá = Giá cơ bản × Hệ số ghế × Hệ số định dạng

Hệ số ghế:  Regular = 1.0 | VIP = 1.5 | Couple = 2.0
Hệ số format: 2D = 1.0 | 3D = 1.2 | 4DX/IMAX = 1.5
```
