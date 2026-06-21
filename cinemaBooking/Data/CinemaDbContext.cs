using cinemaBooking.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Data;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

    public DbSet<NguoiDung> NguoiDung => Set<NguoiDung>();
    public DbSet<TheLoai> TheLoai => Set<TheLoai>();
    public DbSet<Phim> Phim => Set<Phim>();
    public DbSet<TheLoaiPhim> TheLoaiPhim => Set<TheLoaiPhim>();
    public DbSet<RapChieu> RapChieu => Set<RapChieu>();
    public DbSet<PhongChieu> PhongChieu => Set<PhongChieu>();
    public DbSet<GheNgoi> GheNgoi => Set<GheNgoi>();
    public DbSet<SuatChieu> SuatChieu => Set<SuatChieu>();
    public DbSet<DatVe> DatVe => Set<DatVe>();
    public DbSet<ChiTietGheDat> ChiTietGheDat => Set<ChiTietGheDat>();
    public DbSet<ThanhToan> ThanhToan => Set<ThanhToan>();
    public DbSet<BannerQuangCao> BannerQuangCao => Set<BannerQuangCao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map EF Core classes to specific singular Vietnamese table names and configure column lengths
        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.ToTable("NguoiDung");
            entity.Property(e => e.HoTen).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.AnhDaiDien).HasMaxLength(500);
            entity.Property(e => e.VaiTro).HasMaxLength(20);
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.ToTable("TheLoai");
            entity.Property(e => e.TenTheLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.ToTable("Phim");
            entity.Property(e => e.TenPhim).HasMaxLength(250);
            entity.Property(e => e.TenGoc).HasMaxLength(250);
            entity.Property(e => e.MoTa).HasMaxLength(1000);
            entity.Property(e => e.NgonNgu).HasMaxLength(50);
            entity.Property(e => e.QuocGia).HasMaxLength(100);
            entity.Property(e => e.DaoDien).HasMaxLength(150);
            entity.Property(e => e.DienVien).HasMaxLength(500);
            entity.Property(e => e.AnhPoster).HasMaxLength(500);
            entity.Property(e => e.LinkTrailer).HasMaxLength(500);
            entity.Property(e => e.DoTuoiQuyDinh).HasMaxLength(10);
            entity.Property(e => e.TrangThaiChieu).HasMaxLength(20);
        });

        modelBuilder.Entity<TheLoaiPhim>().ToTable("TheLoaiPhim");

        modelBuilder.Entity<RapChieu>(entity =>
        {
            entity.ToTable("RapChieu");
            entity.Property(e => e.TenRap).HasMaxLength(150);
            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.ThanhPho).HasMaxLength(100);
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HinhAnhRap).HasMaxLength(500);
        });

        modelBuilder.Entity<PhongChieu>(entity =>
        {
            entity.ToTable("PhongChieu");
            entity.Property(e => e.TenPhong).HasMaxLength(100);
            entity.Property(e => e.LoaiPhong).HasMaxLength(50);
        });

        modelBuilder.Entity<GheNgoi>(entity =>
        {
            entity.ToTable("GheNgoi");
            entity.Property(e => e.HangGhe).HasMaxLength(10);
            entity.Property(e => e.LoaiGhe).HasMaxLength(50);
        });

        modelBuilder.Entity<SuatChieu>(entity =>
        {
            entity.ToTable("SuatChieu");
            entity.Property(e => e.PhongDich).HasMaxLength(50);
            entity.Property(e => e.DinhDang).HasMaxLength(50);
            entity.Property(e => e.TrangThaiSuat).HasMaxLength(20);
        });

        modelBuilder.Entity<DatVe>(entity =>
        {
            entity.ToTable("DatVe");
            entity.Property(e => e.MaGiaoDich).HasMaxLength(50);
            entity.Property(e => e.TrangThaiDat).HasMaxLength(20);
        });

        modelBuilder.Entity<ChiTietGheDat>().ToTable("ChiTietGheDat");

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.ToTable("ThanhToan");
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.MaGiaoDichNganHang).HasMaxLength(100);
            entity.Property(e => e.TrangThaiThanhToan).HasMaxLength(20);
        });

        modelBuilder.Entity<BannerQuangCao>(entity =>
        {
            entity.ToTable("BannerQuangCao");
            entity.Property(e => e.TieuDe).HasMaxLength(250);
            entity.Property(e => e.DuongDanAnh).HasMaxLength(500);
            entity.Property(e => e.DuongDanLienKet).HasMaxLength(500);
        });

        // Unique indexes
        modelBuilder.Entity<NguoiDung>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<DatVe>()
            .HasIndex(b => b.MaGiaoDich)
            .IsUnique();

        // Composite PK: TheLoaiPhim
        modelBuilder.Entity<TheLoaiPhim>()
            .HasKey(mg => new { mg.MaPhim, mg.MaTheLoai });

        // Composite unique: Seat per room
        modelBuilder.Entity<GheNgoi>()
            .HasIndex(s => new { s.MaPhong, s.HangGhe, s.SoGhe })
            .IsUnique();

        // Composite unique: ChiTietGheDat
        modelBuilder.Entity<ChiTietGheDat>()
            .HasIndex(bs => new { bs.MaDatVe, bs.MaGhe })
            .IsUnique();

        // Payment 1:1 with Booking
        modelBuilder.Entity<ThanhToan>()
            .HasOne(p => p.DatVe)
            .WithOne(b => b.ThanhToan)
            .HasForeignKey<ThanhToan>(p => p.MaDatVe)
            .OnDelete(DeleteBehavior.Cascade);

        // Booking -> User / Showtime: no cascade
        modelBuilder.Entity<DatVe>()
            .HasOne(b => b.NguoiDung)
            .WithMany(u => u.DatVes)
            .HasForeignKey(b => b.MaNguoiDung)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DatVe>()
            .HasOne(b => b.SuatChieu)
            .WithMany(s => s.DatVes)
            .HasForeignKey(b => b.MaSuatChieu)
            .OnDelete(DeleteBehavior.Restrict);

        // Showtime -> Room: no cascade
        modelBuilder.Entity<SuatChieu>()
            .HasOne(s => s.PhongChieu)
            .WithMany(r => r.SuatChieus)
            .HasForeignKey(s => s.MaPhong)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision
        modelBuilder.Entity<DatVe>()
            .Property(b => b.TongTien)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ThanhToan>()
            .Property(p => p.SoTien)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<SuatChieu>()
            .Property(s => s.GiaVeCoBan)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ChiTietGheDat>()
            .Property(bs => bs.GiaVeThucTe)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Phim>()
            .Property(m => m.DiemDanhGia)
            .HasColumnType("decimal(3,1)");
    }
}
