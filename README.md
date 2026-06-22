# Cinema Star – Hệ Thống Đặt Vé Xem Phim Trực Tuyến

Dự án ASP.NET Core 8 MVC quản lý và đặt vé xem phim trực tuyến.

- **Tên sinh viên:** Vũ Khắc Diệp
- **Mã đồ án:** DK24TT80171

---

## 1. Tổng Quan Hệ Thống

Cinema Star là ứng dụng web cho phép người dùng duyệt tìm danh sách phim, xem thông tin chi tiết và trailer phim, xem lịch chiếu, chọn ghế trực quan theo sơ đồ phòng chiếu thời gian thực và đặt vé trực tuyến. Hệ thống được thiết kế tối ưu và chia làm hai phân quyền chính: **Khách Hàng (Customer)** và **Quản Trị Viên (Admin)**.

### Tính năng chính:

- **Khách vãng lai:** Xem danh sách phim đang chiếu/sắp chiếu, tìm kiếm phim, lọc phim theo thể loại, xem chi tiết lịch chiếu và đăng ký tài khoản.
- **Khách hàng:** Đăng nhập, quản lý hồ sơ cá nhân, đổi mật khẩu, đặt vé xem phim, chọn ghế trực quan trên sơ đồ phòng chiếu, thực hiện thanh toán trực tuyến (thanh toán giả lập), xem lịch sử đặt vé và hủy vé trước giờ chiếu.
- **Quản trị viên (Admin):**
  - Xem Dashboard thống kê: Tổng doanh thu, tổng số phim, phim đang chiếu, số lượng đơn đặt vé, số lượng người dùng, biểu đồ 5 phim ăn khách nhất và danh sách đơn đặt vé mới nhất.
  - Quản lý Phim (CRUD).
  - Quản lý Suất chiếu (CRUD).
  - Quản lý Người dùng (khóa/mở khóa tài khoản, đổi vai trò người dùng).
  - Quản lý đơn đặt vé (Confirmed, Pending, Cancelled).
  - Quản lý Cụm rạp chiếu.

---

## 2. Stack Công Nghệ Sử Dụng

| Thành phần         | Công nghệ                        | Chi tiết                                                  |
| :----------------- | :------------------------------- | :-------------------------------------------------------- |
| **Framework**      | ASP.NET Core 8.0 MVC             | Mô hình Model-View-Controller hiện đại của Microsoft      |
| **Ngôn ngữ**       | C# 12                            | Hỗ trợ các cú pháp bất đồng bộ (async/await, LINQ)        |
| **ORM**            | Entity Framework Core 8          | Tương tác CSDL bằng LINQ, quản lý dữ liệu qua DbContext   |
| **Database**       | SQL Server (LocalDB mặc định)    | Hệ quản trị cơ sở dữ liệu quan hệ cục bộ                  |
| **Authentication** | Cookie Authentication            | Cơ chế xác thực phân quyền qua Claims Identity Cookie     |
| **Mật khẩu**       | Plain Text                       | Lưu mật khẩu văn bản thuần túy phục vụ giáo viên kiểm tra |
| **UI Styling**     | Bootstrap 5.3 + Font Awesome 6.4 | Thiết kế responsive, biểu tượng động hiện đại             |
| **Fonts**          | Google Fonts (Outfit)            | Font chữ tinh tế, hiện đại cho giao diện Cinema tối màu   |

---

## 3. Cấu Trúc Thư Mục Dự Án

```
ASPNET-DK24TT80171-VuKhacDiep-banvexemfilm/
├── cinemaBooking/
│   ├── Controllers/
│   │   ├── HomeController.cs          # Định tuyến: / (Trang chủ, chính sách, báo lỗi)
│   │   ├── AccountController.cs       # Định tuyến: /tai-khoan (Đăng nhập, đăng ký, hồ sơ, đổi mật khẩu)
│   │   ├── MovieController.cs         # Định tuyến: /phim (Danh sách phim, chi tiết phim, thêm/sửa/xóa phim)
│   │   ├── BookingController.cs       # Định tuyến: /dat-ve (Chọn ghế, tạo đơn, xác nhận, thanh toán, lịch sử, hủy)
│   │   └── AdminController.cs         # Định tuyến: /quan-tri (Dashboard, người dùng, phim, suất chiếu, đơn hàng, rạp)
│   ├── Data/
│   │   ├── CinemaDbContext.cs         # EF Core DbContext cấu hình Fluent API Việt hóa
│   │   └── SeedData.cs                # Nạp dữ liệu mẫu bằng tiếng Việt (người dùng, phim, rạp, phòng, ghế, suất chiếu)
│   ├── Models/
│   │   ├── Domain/                    # 12 Entity Models thực thể (Việt hóa toàn bộ)
│   │   │   ├── NguoiDung.cs           # Thông tin người dùng
│   │   │   ├── Phim.cs                # Thông tin phim
│   │   │   ├── TheLoai.cs             # Thể loại phim
│   │   │   ├── TheLoaiPhim.cs         # Bảng trung gian Nhiều - Nhiều (Phim <-> Thể loại)
│   │   │   ├── RapChieu.cs            # Thông tin cụm rạp
│   │   │   ├── PhongChieu.cs          # Thông tin phòng chiếu của rạp
│   │   │   ├── GheNgoi.cs             # Sơ đồ ghế ngồi của phòng chiếu
│   │   │   ├── SuatChieu.cs           # Lịch chiếu phim
│   │   │   ├── DatVe.cs               # Đơn hàng đặt vé xem phim
│   │   │   ├── ChiTietGheDat.cs       # Danh sách chi tiết các ghế trong đơn đặt vé
│   │   │   ├── ThanhToan.cs           # Giao dịch thanh toán hóa đơn
│   │   │   └── BannerQuangCao.cs      # Banners quảng cáo trên trang chủ
│   │   └── ViewModels/                # ViewModels truyền dữ liệu sang View
│   │       ├── AccountViewModels.cs
│   │       ├── MovieViewModels.cs
│   │       ├── BookingViewModels.cs
│   │       ├── ShowtimeViewModels.cs
│   │       └── DashboardViewModels.cs
│   ├── Services/
│   │   ├── Interfaces/                # Interfaces định nghĩa các Services
│   │   │   ├── IUserService.cs
│   │   │   ├── IMovieService.cs
│   │   │   ├── ICinemaService.cs
│   │   │   ├── IShowtimeService.cs
│   │   │   └── IBookingService.cs
│   │   ├── UserService.cs             # Xử lý logic nghiệp vụ người dùng (xác thực plain text)
│   │   ├── MovieService.cs            # Xử lý dữ liệu phim và thể loại
│   │   ├── CinemaService.cs           # Xử lý thông tin rạp chiếu
│   │   ├── ShowtimeService.cs         # Cấu hình suất chiếu và sơ đồ ghế ngồi
│   │   └── BookingService.cs          # Xử lý logic đặt vé, tính tiền động, thanh toán, lịch sử, hủy vé
│   ├── Views/
│   │   ├── Account/  (Login, Register, Profile, ChangePassword, AccessDenied)
│   │   ├── Admin/    (Dashboard, Movies, Showtimes, Bookings, Users, Cinemas, _AdminSidebar)
│   │   ├── Booking/  (SeatSelection, Confirmation, History)
│   │   ├── Home/     (Index, Privacy)
│   │   ├── Movie/    (Index, Details, Create, Edit)
│   │   └── Shared/   (_Layout, Error, _ValidationScriptsPartial)
│   ├── wwwroot/
│   │   ├── css/site.css               # CSS tùy biến tối màu chuẩn Cinema Star
│   │   ├── img/                       # Các tài nguyên hình ảnh logo, icon
│   │   └── js/site.js                 # Javascript xử lý tương tác client-side
│   ├── Program.cs                     # Cấu hình Services, Cookie Auth, Pipeline và Routing
│   ├── appsettings.json               # Chuỗi kết nối Database và cấu hình chung
│   └── cinemaBooking.csproj           # Cấu hình gói thư viện NuGet của dự án
└── README.md
```

---

## 4. Hướng Dẫn Cài Đặt & Khởi Chạy Lần Đầu

### Bước 1: Mở thư mục dự án

Mở thư mục `cinemaBooking` bằng Microsoft Visual Studio 2022 hoặc Visual Studio Code.

### Bước 2: Cấu hình Connection String

Mở file `appsettings.json` trong thư mục `cinemaBooking` và kiểm tra chuỗi kết nối phù hợp với máy của bạn:

- **Sử dụng SQL Server LocalDB mặc định (Khuyên dùng):**
  ```json
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CinemaBookingDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  ```
- **Sử dụng SQL Server Express:**
  ```json
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=CinemaBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
  ```
- **Sử dụng SQL Server Instance riêng:**
  ```json
  "DefaultConnection": "Server=TEN_MAY_TINH\\TEN_INSTANCE;Database=CinemaBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
  ```

### Bước 3: Khởi chạy dự án

Mở Terminal tại thư mục `cinemaBooking` và chạy lệnh sau để khởi động dự án:

```bash
dotnet run
```

Hoặc để tự động cập nhật giao diện khi sửa code UI:

```bash
dotnet watch run
```

Khi dự án khởi chạy lần đầu tiên, hệ thống sẽ tự động:

1. Kiểm tra và tự động khởi tạo cơ sở dữ liệu `CinemaBookingDB` thông qua `context.Database.EnsureCreated()`.
2. Tự động nạp dữ liệu mẫu (Seed Data) bao gồm các tài khoản dùng thử, danh sách phim, danh sách rạp, danh sách phòng chiếu, cấu hình ghế ngồi và các suất chiếu demo vào Database.

Đường dẫn truy cập cục bộ mặc định: **`http://localhost:5025`**

---

## 5. Danh Sách Tài Khoản Demo

Hệ thống đã chuẩn bị sẵn các tài khoản demo trong `SeedData.cs` với mật khẩu dạng văn bản thuần túy (Plain Text) như sau:

| Tên hiển thị       | Email đăng nhập    | Mật khẩu       | Vai trò      | Trạng thái tài khoản    |
| :----------------- | :----------------- | :------------- | :----------- | :---------------------- |
| **Quản Trị Viên**  | `admin@cinema.com` | `Admin@123`    | **Admin**    | Đang hoạt động (Active) |
| **Vũ Khắc Diệp**   | `diep@cinema.com`  | `Customer@123` | **Customer** | Đang hoạt động (Active) |
| **Nguyễn Thị Lan** | `lan@cinema.com`   | `Customer@123` | **Customer** | Đang hoạt động (Active) |
| **Trần Văn Hùng**  | `hung@cinema.com`  | `Customer@123` | **Customer** | Đang hoạt động (Active) |
| **Phạm Minh Tuấn** | `tuan@cinema.com`  | `Customer@123` | **Customer** | Đang hoạt động (Active) |
| **Lê Thị Mai**     | `mai@cinema.com`   | `Customer@123` | **Customer** | **Bị khóa (Inactive)**  |

---

## 6. Lộ Trình Kiểm Thử Nghiệp Vụ (End-to-End Test Flow)

### Luồng 1 – Khách hàng Đặt vé & Thanh toán

1. Truy cập Trang chủ `/` → Xem các bộ phim đang chiếu.
2. Nhấp vào nút xem chi tiết bộ phim → URL chuyển sang `/phim/chi-tiet/{id}`.
3. Chọn suất chiếu mong muốn trong phần lịch chiếu.
4. Hệ thống yêu cầu đăng nhập nếu chưa đăng nhập → Tự động chuyển đến trang Đăng nhập `/tai-khoan/dang-nhap`.
5. Đăng nhập bằng tài khoản `diep@cinema.com` / `Customer@123`.
6. Sau khi đăng nhập thành công, hệ thống tự động quay lại trang Chọn ghế `/dat-ve/chon-ghe?showtimeId=...`.
7. Tiến hành nhấp chọn các ghế trên sơ đồ phòng chiếu (ví dụ: A1, A2...) → Nhấp nút **Tiếp tục**.
8. Trang Xác nhận thông tin đơn hàng hiển thị `/dat-ve/xac-nhan/{id}` → Nhấp nút **Thanh toán ngay**.
9. Thông báo thanh toán thành công hiển thị. Xem lại vé đã đặt tại trang Lịch sử đặt vé `/dat-ve/lich-su-dat-ve`.

### Luồng 2 – Đăng ký thành viên mới

1. Truy cập trang Đăng ký thành viên `/tai-khoan/dang-ky`.
2. Điền đầy đủ thông tin: Họ tên, Email, Số điện thoại, Mật khẩu → Nhấp nút **Đăng ký**.
3. Hệ thống chuyển hướng sang trang Đăng nhập `/tai-khoan/dang-nhap`. Đăng nhập bằng tài khoản vừa tạo để trải nghiệm.

### Luồng 3 – Quản trị viên quản lý hệ thống

1. Đăng nhập tài khoản Admin: `admin@cinema.com` / `Admin@123`.
2. Truy cập Bảng điều khiển quản trị `/quan-tri/bang-dieu-khien` để xem báo cáo thống kê doanh thu và phim hot.
3. Vào mục **Quản lý phim** `/quan-tri/phim` → Thử thêm phim mới, sửa thông tin phim hoặc xóa phim.
4. Vào mục **Suất chiếu** `/quan-tri/suat-chieu` → Thử tạo suất chiếu mới cho phim tại các phòng chiếu đã có.
5. Vào mục **Người dùng** `/quan-tri/nguoi-dung` → Thử thay đổi vai trò (Role) hoặc bấm khóa/mở khóa tài khoản người dùng khác.

### Luồng 4 – Hủy đơn đặt vé

1. Đăng nhập bằng tài khoản Khách hàng.
2. Truy cập trang Lịch sử đặt vé `/dat-ve/lich-su-dat-ve`.
3. Tìm đơn đặt vé có trạng thái **Confirmed** (Đã xác nhận) → Nhấp vào nút **Hủy vé**.
4. Trạng thái đơn đặt vé chuyển sang **Cancelled** (Đã hủy), trạng thái thanh toán chuyển sang **Refunded** (Đã hoàn tiền).

---

## 7. Thiết Kế Cơ Sở Dữ Liệu & Các Mối Quan Hệ

Cơ sở dữ liệu được Việt hóa hoàn toàn gồm 12 bảng thực thể liên kết chặt chẽ với nhau:

```
NguoiDung ───────────────────────── DatVe
  │                                    │
  │ (VaiTro: Admin / Khác hàng)        ├── ChiTietGheDat ── GheNgoi ── PhongChieu ── RapChieu
  │                                    │
  │                                    └── ThanhToan
  │
  Phim ──── TheLoaiPhim ──── TheLoai
  │
  └── SuatChieu ──── PhongChieu

BannerQuangCao (Độc lập)
```

---

## 8. Logic Tính Giá Vé Động

Giá vé thực tế cho từng vị trí ghế được hệ thống tự động tính toán và làm tròn tại `BookingService.cs` theo công thức:

$$\text{Giá vé thực tế} = \text{Giá vé cơ bản} \times \text{Hệ số loại ghế} \times \text{Hệ số định dạng phim}$$

### Bảng hệ số cấu hình:

1. **Hệ số loại ghế:**
   - Ghế thường (`Regular`): $1.0$
   - Ghế VIP (`VIP`): $1.5$
   - Ghế đôi (`Couple`): $2.0$
2. **Hệ số định dạng phim:**
   - Phim định dạng 3D (`3D`): $1.2$
   - Phim định dạng khác (2D, 2D Lồng tiếng...): $1.0$

_Ví dụ: Nếu suất chiếu phim 3D có giá cơ bản là $80,000$ VNĐ, khách hàng chọn ghế VIP (hệ số 1.5). Giá vé thực tế tính được là:_
$$80,000 \times 1.5 \times 1.2 = 144,000 \text{ VNĐ}$$

_Kết quả sau đó được làm tròn đến hàng nghìn đồng bằng hàm `Math.Round(price / 1000) _ 1000` trước khi lưu vào cơ sở dữ liệu.\*
