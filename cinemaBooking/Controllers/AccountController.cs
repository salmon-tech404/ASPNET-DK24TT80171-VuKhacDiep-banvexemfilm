using System.Security.Claims;
using cinemaBooking.Models.ViewModels;
using cinemaBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cinemaBooking.Controllers;

[Route("tai-khoan")]
public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("dang-nhap")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("dang-nhap")]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userService.AuthenticateAsync(model.Email, model.MatKhau);
        if (user == null)
        {
            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.HoTen),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.VaiTro),
            new("AvatarUrl", user.AnhDaiDien ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var props = new AuthenticationProperties { IsPersistent = model.RememberMe };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

        TempData["Success"] = $"Chào mừng trở lại, {user.HoTen}!";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        if (user.VaiTro == "Admin")
            return RedirectToAction("Dashboard", "Admin");

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("dang-ky")]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("dang-ky")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _userService.EmailExistsAsync(model.Email))
        {
            ModelState.AddModelError("Email", "Email này đã được đăng ký");
            return View(model);
        }

        var user = await _userService.RegisterAsync(model.HoTen, model.Email, model.MatKhau, model.DienThoai);
        if (user == null)
        {
            ModelState.AddModelError("", "Đăng ký thất bại, vui lòng thử lại");
            return View(model);
        }

        TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("dang-xuat")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Success"] = "Đăng xuất thành công.";
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    [Route("ho-so")]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        return View(new ProfileViewModel
        {
            HoTen = user.HoTen,
            Email = user.Email,
            DienThoai = user.DienThoai,
            AnhDaiDien = user.AnhDaiDien
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("ho-so")]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _userService.UpdateProfileAsync(userId, model.HoTen, model.DienThoai, model.AnhDaiDien);
        TempData["Success"] = "Cập nhật thông tin thành công.";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
    [HttpGet]
    [Route("doi-mat-khau")]
    public IActionResult ChangePassword() => View();

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("doi-mat-khau")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _userService.ChangePasswordAsync(userId, model.MatKhauHienTai, model.MatKhauMoi);

        if (!result)
        {
            ModelState.AddModelError("MatKhauHienTai", "Mật khẩu hiện tại không đúng");
            return View(model);
        }

        TempData["Success"] = "Đổi mật khẩu thành công.";
        return RedirectToAction(nameof(Profile));
    }

    [Route("tu-choi-truy-cap")]
    public IActionResult AccessDenied() => View();
}
