using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cinemaBooking.Controllers;

[Route("phim")]
public class MovieController : Controller
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Route("")]
    public async Task<IActionResult> Index(string? search, string? status, int? genreId)
    {
        var movies = await _movieService.GetMoviesAsync(search, status, genreId);
        var genres = await _movieService.GetAllGenresAsync();

        var vm = new MovieListPageViewModel
        {
            Phims = movies,
            SearchQuery = search,
            StatusFilter = status,
            GenreFilter = genreId,
            TheLoais = genres.Select(g => (g.Id, g.TenTheLoai)).ToList()
        };
        return View(vm);
    }

    [Route("chi-tiet/{id}")]
    public async Task<IActionResult> Details(int id, DateTime? date)
    {
        var movie = await _movieService.GetMovieDetailAsync(id, date);
        if (movie == null) return NotFound();
        ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
        return View(movie);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("them-phim")]
    public async Task<IActionResult> Create()
    {
        var vm = await _movieService.GetMovieFormAsync();
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("them-phim")]
    public async Task<IActionResult> Create(MovieFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var allGenres = await _movieService.GetAllGenresAsync();
            model.TatCaTheLoai = allGenres.Select(g => new GenreCheckboxItem { Id = g.Id, TenTheLoai = g.TenTheLoai, IsChecked = model.MaTheLoaisDaChon.Contains(g.Id) }).ToList();
            return View(model);
        }

        var movie = await _movieService.CreateMovieAsync(model);
        TempData["Success"] = $"Thêm phim '{movie.TenPhim}' thành công.";
        return RedirectToAction("Movies", "Admin");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("sua-phim/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _movieService.GetMovieFormAsync(id);
        if (vm.Id == 0) return NotFound();
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("sua-phim/{id}")]
    public async Task<IActionResult> Edit(int id, MovieFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var allGenres = await _movieService.GetAllGenresAsync();
            model.TatCaTheLoai = allGenres.Select(g => new GenreCheckboxItem { Id = g.Id, TenTheLoai = g.TenTheLoai, IsChecked = model.MaTheLoaisDaChon.Contains(g.Id) }).ToList();
            return View(model);
        }

        await _movieService.UpdateMovieAsync(id, model);
        TempData["Success"] = "Cập nhật phim thành công.";
        return RedirectToAction("Movies", "Admin");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("xoa-phim/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieService.DeleteMovieAsync(id);
        TempData["Success"] = "Đã xóa phim.";
        return RedirectToAction("Movies", "Admin");
    }
}
