using System.Security.Claims;
using BuoiToi.Data;
using BuoiToi.Models;
using BuoiToi.Models.Enums;
using BuoiToi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuoiToi.Controllers;


public class AccessController : Controller
{
    private readonly SetupDatabase _db;

    public AccessController(SetupDatabase db)
    {
        _db = db;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        var claimsPrincipal = HttpContext.User;
        if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
        {
            return Redirect("/Product");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == request.Username && user.Password == request.Password);
        if (user == null)
        {
            ViewBag.Message = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, request.Username),
            new (ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new()
        {
            AllowRefresh = true,
            IsPersistent = request.RememberMe
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), properties);
        var total = await (from bd in _db.BillDetails
            join b in _db.Bills on bd.BillId equals b.Id
            where b.Status == StatusBill.CART && b.UserId == user.Id
            select bd.Quantity).SumAsync();
        return Redirect("/Product?total="+ total);

    }
    
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost("/Register")]
    public async Task<IActionResult> Register(User user)
    {
        var exitsUser = await _db.Users.AnyAsync(u => u.Username == user.Username);
        if(exitsUser)
        {
            ViewBag.Errors = "Username đã tồn tại";
            return View();
        }
        user.Role = "User";
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return RedirectToAction("Login");
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}