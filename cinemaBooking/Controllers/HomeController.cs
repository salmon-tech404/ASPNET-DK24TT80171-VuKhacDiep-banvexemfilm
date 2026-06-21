using System.Diagnostics;
using cinemaBooking.Models;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cinemaBooking.Controllers;

[Route("")]
public class HomeController : Controller
{
    private readonly IMovieService _movieService;

    public HomeController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var nowShowing = await _movieService.GetMoviesAsync(status: "NowShowing");
        var comingSoon = await _movieService.GetMoviesAsync(status: "ComingSoon");

        var vm = new HomeViewModel
        {
            PhimDangChieu = nowShowing.Take(8).ToList(),
            PhimSapChieu = comingSoon.Take(4).ToList()
        };
        return View(vm);
    }

    [Route("chinh-sach-bao-mat")]
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("loi")]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
