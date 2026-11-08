using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Helpers;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _api;
        public AccountController(ApiClient api) => _api = api;

        // ---------- LOGIN ----------
        [HttpGet]
        public IActionResult Login()
        {
            if (AuthCheck.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", AuthCheck.IsOrganiser(HttpContext.Session) ? "Organiser" : "Participant");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var r = await _api.PostAsync("api/auth/login", new { email = model.Email, password = model.Password });
            if (!r.IsSuccessStatusCode) { ViewBag.Error = "Invalid email or password."; return View(model); }

            var json = await r.Content.ReadAsStringAsync();
            var tok = JsonSerializer.Deserialize<AuthTokenResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            HttpContext.Session.SetString("JwtToken", tok.Token);

            // Second call: fetch who we are
            var meR = await _api.GetAsync("api/users/me");
            if (!meR.IsSuccessStatusCode) { ViewBag.Error = "Could not load profile."; return View(model); }

            var meJson = await meR.Content.ReadAsStringAsync();
            var me = JsonSerializer.Deserialize<MeResponse>(meJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            HttpContext.Session.SetInt32("UserId", me.UserId);
            HttpContext.Session.SetString("FullName", me.FullName);
            HttpContext.Session.SetString("Email", me.Email);
            HttpContext.Session.SetString("Role", me.Role);

            return me.Role == "Organiser"
                ? RedirectToAction("Index", "Organiser")
                : RedirectToAction("Index", "Participant");
        }

        // ---------- REGISTER ----------
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var r = await _api.PostAsync("api/auth/register", new
            {
                fullName = model.FullName,
                email = model.Email,
                password = model.Password,
                role = "Participant",   // API ignores it but DTO requires it
                phone = model.Phone
            });

            if (!r.IsSuccessStatusCode)
            {
                ViewBag.Error = await r.Content.ReadAsStringAsync();
                return View(model);
            }
            return RedirectToAction("Login");
        }

        // ---------- LOGOUT ----------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ---------- ERROR PAGE ----------
        public IActionResult Error() => View();
    }
}