using cinemaBooking.Data;
using cinemaBooking.Models.Domain;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cinemaBooking.Services;

public class MovieService : IMovieService
{
    private readonly CinemaDbContext _context;

    public MovieService(CinemaDbContext context) => _context = context;

    public async Task<List<MovieCardViewModel>> GetMoviesAsync(string? search = null, string? status = null, int? genreId = null)
    {
        var query = _context.Phim
            .Include(m => m.TheLoaiPhims).ThenInclude(mg => mg.TheLoai)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.TenPhim.Contains(search) || (m.TenGoc != null && m.TenGoc.Contains(search)));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(m => m.TrangThaiChieu == status);

        if (genreId.HasValue)
            query = query.Where(m => m.TheLoaiPhims.Any(mg => mg.MaTheLoai == genreId.Value));

        var movies = await query.OrderByDescending(m => m.NgayTao).ToListAsync();
        return movies.Select(ToCardViewModel).ToList();
    }

    public async Task<MovieDetailViewModel?> GetMovieDetailAsync(int movieId, DateTime? date = null)
    {
        var movie = await _context.Phim
            .Include(m => m.TheLoaiPhims).ThenInclude(mg => mg.TheLoai)
            .Include(m => m.SuatChieus)
                .ThenInclude(s => s.PhongChieu)
                    .ThenInclude(r => r.RapChieu)
            .Include(m => m.SuatChieus)
                .ThenInclude(s => s.DatVes)
                    .ThenInclude(b => b.ChiTietGheDats)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null) return null;

        var filterDate = date?.Date ?? DateTime.Today;
        var nextDate = filterDate.AddDays(1);

        var filteredShowtimes = movie.SuatChieus
            .Where(s => s.TrangThaiSuat != "Cancelled" && s.GioBatDau >= filterDate && s.GioBatDau < nextDate)
            .ToList();

        var cinemaGroups = filteredShowtimes
            .GroupBy(s => s.PhongChieu.RapChieu)
            .Select(g => new ShowtimeGroupViewModel
            {
                TenRap = g.Key.TenRap,
                ThanhPho = g.Key.ThanhPho,
                SuatChieus = g.Select(s =>
                {
                    var bookedSeats = s.DatVes
                        .Where(b => b.TrangThaiDat != "Cancelled")
                        .SelectMany(b => b.ChiTietGheDats)
                        .Count();
                    return new ShowtimeItemViewModel
                    {
                        Id = s.Id,
                        GioBatDau = s.GioBatDau,
                        GioKetThuc = s.GioKetThuc,
                        DinhDang = s.DinhDang,
                        PhongDich = s.PhongDich,
                        GiaVeCoBan = s.GiaVeCoBan,
                        TenPhong = s.PhongChieu.TenPhong,
                        TrangThaiSuat = s.TrangThaiSuat,
                        SoGheTrong = s.PhongChieu.TongSoGhe - bookedSeats,
                        TongSoGhe = s.PhongChieu.TongSoGhe
                    };
                }).OrderBy(s => s.GioBatDau).ToList()
            }).ToList();

        return new MovieDetailViewModel
        {
            Id = movie.Id,
            TenPhim = movie.TenPhim,
            TenGoc = movie.TenGoc,
            MoTa = movie.MoTa,
            ThoiLuong = movie.ThoiLuong,
            NgayChieu = movie.NgayChieu,
            NgayKetThuc = movie.NgayKetThuc,
            NgonNgu = movie.NgonNgu,
            QuocGia = movie.QuocGia,
            DaoDien = movie.DaoDien,
            DienVien = movie.DienVien,
            AnhPoster = movie.AnhPoster,
            LinkTrailer = movie.LinkTrailer,
            DiemDanhGia = movie.DiemDanhGia,
            DoTuoiQuyDinh = movie.DoTuoiQuyDinh,
            TrangThaiChieu = movie.TrangThaiChieu,
            TheLoais = movie.TheLoaiPhims.Select(mg => mg.TheLoai.TenTheLoai).ToList(),
            NhomSuatChieus = cinemaGroups
        };
    }

    public async Task<Phim?> GetMovieByIdAsync(int movieId) =>
        await _context.Phim.Include(m => m.TheLoaiPhims).FirstOrDefaultAsync(m => m.Id == movieId);

    public async Task<Phim> CreateMovieAsync(MovieFormViewModel model)
    {
        var movie = new Phim
        {
            TenPhim = model.TenPhim,
            TenGoc = model.TenGoc,
            MoTa = model.MoTa,
            ThoiLuong = model.ThoiLuong,
            NgayChieu = model.NgayChieu,
            NgayKetThuc = model.NgayKetThuc,
            NgonNgu = model.NgonNgu,
            QuocGia = model.QuocGia,
            DaoDien = model.DaoDien,
            DienVien = model.DienVien,
            AnhPoster = model.AnhPoster,
            LinkTrailer = model.LinkTrailer,
            DiemDanhGia = model.DiemDanhGia,
            DoTuoiQuyDinh = model.DoTuoiQuyDinh,
            TrangThaiChieu = model.TrangThaiChieu,
            NgayTao = DateTime.Now,
            NgayCapNhat = DateTime.Now
        };
        _context.Phim.Add(movie);
        await _context.SaveChangesAsync();

        foreach (var genreId in model.MaTheLoaisDaChon)
            _context.TheLoaiPhim.Add(new TheLoaiPhim { MaPhim = movie.Id, MaTheLoai = genreId });
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<bool> UpdateMovieAsync(int movieId, MovieFormViewModel model)
    {
        var movie = await _context.Phim.Include(m => m.TheLoaiPhims).FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null) return false;

        movie.TenPhim = model.TenPhim;
        movie.TenGoc = model.TenGoc;
        movie.MoTa = model.MoTa;
        movie.ThoiLuong = model.ThoiLuong;
        movie.NgayChieu = model.NgayChieu;
        movie.NgayKetThuc = model.NgayKetThuc;
        movie.NgonNgu = model.NgonNgu;
        movie.QuocGia = model.QuocGia;
        movie.DaoDien = model.DaoDien;
        movie.DienVien = model.DienVien;
        movie.AnhPoster = model.AnhPoster;
        movie.LinkTrailer = model.LinkTrailer;
        movie.DiemDanhGia = model.DiemDanhGia;
        movie.DoTuoiQuyDinh = model.DoTuoiQuyDinh;
        movie.TrangThaiChieu = model.TrangThaiChieu;
        movie.NgayCapNhat = DateTime.Now;

        _context.TheLoaiPhim.RemoveRange(movie.TheLoaiPhims);
        foreach (var genreId in model.MaTheLoaisDaChon)
            _context.TheLoaiPhim.Add(new TheLoaiPhim { MaPhim = movieId, MaTheLoai = genreId });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await _context.Phim.FindAsync(movieId);
        if (movie == null) return false;
        _context.Phim.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<TheLoai>> GetAllGenresAsync() =>
        await _context.TheLoai.OrderBy(g => g.TenTheLoai).ToListAsync();

    public async Task<MovieFormViewModel> GetMovieFormAsync(int? movieId = null)
    {
        var allGenres = await GetAllGenresAsync();
        var vm = new MovieFormViewModel
        {
            TatCaTheLoai = allGenres.Select(g => new GenreCheckboxItem { Id = g.Id, TenTheLoai = g.TenTheLoai }).ToList()
        };

        if (movieId.HasValue)
        {
            var movie = await GetMovieByIdAsync(movieId.Value);
            if (movie != null)
            {
                vm.Id = movie.Id;
                vm.TenPhim = movie.TenPhim;
                vm.TenGoc = movie.TenGoc;
                vm.MoTa = movie.MoTa;
                vm.ThoiLuong = movie.ThoiLuong;
                vm.NgayChieu = movie.NgayChieu;
                vm.NgayKetThuc = movie.NgayKetThuc;
                vm.NgonNgu = movie.NgonNgu;
                vm.QuocGia = movie.QuocGia;
                vm.DaoDien = movie.DaoDien;
                vm.DienVien = movie.DienVien;
                vm.AnhPoster = movie.AnhPoster;
                vm.LinkTrailer = movie.LinkTrailer;
                vm.DiemDanhGia = movie.DiemDanhGia;
                vm.DoTuoiQuyDinh = movie.DoTuoiQuyDinh;
                vm.TrangThaiChieu = movie.TrangThaiChieu;
                vm.MaTheLoaisDaChon = movie.TheLoaiPhims.Select(mg => mg.MaTheLoai).ToList();

                foreach (var item in vm.TatCaTheLoai)
                    item.IsChecked = vm.MaTheLoaisDaChon.Contains(item.Id);
            }
        }

        return vm;
    }

    private static MovieCardViewModel ToCardViewModel(Phim movie) => new()
    {
        Id = movie.Id,
        TenPhim = movie.TenPhim,
        AnhPoster = movie.AnhPoster,
        ThoiLuong = movie.ThoiLuong,
        DiemDanhGia = movie.DiemDanhGia,
        DoTuoiQuyDinh = movie.DoTuoiQuyDinh,
        TrangThaiChieu = movie.TrangThaiChieu,
        NgayChieu = movie.NgayChieu,
        TheLoais = movie.TheLoaiPhims.Select(mg => mg.TheLoai.TenTheLoai).ToList()
    };
}
