using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Helpers;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiClient _api;
        private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

        public ParticipantController(ApiClient api) => _api = api;

        private bool Guard()
            => AuthCheck.IsParticipant(HttpContext.Session);

        // ---------- BROWSE EVENTS ----------
        public async Task<IActionResult> Index()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync("api/events");
            var json = await r.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<EventVm>>(json, Opts) ?? new();
            return View(events);
        }

        // ---------- EVENT DETAILS ----------
        public async Task<IActionResult> Details(int id)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var evR = await _api.GetAsync($"api/events/{id}");
            if (!evR.IsSuccessStatusCode) return NotFound();
            var evJson = await evR.Content.ReadAsStringAsync();
            var ev = JsonSerializer.Deserialize<EventVm>(evJson, Opts)!;

            var catR = await _api.GetAsync($"api/categories?eventId={id}");
            var catJson = await catR.Content.ReadAsStringAsync();
            var cats = JsonSerializer.Deserialize<List<CategoryVm>>(catJson, Opts) ?? new();

            ViewBag.Categories = cats;
            return View(ev);
        }

        // ---------- ENROL ----------
        [HttpPost]
        public async Task<IActionResult> Enrol(int categoryId, int eventId)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.PostAsync("api/enrolments", new { categoryId });
            TempData[r.IsSuccessStatusCode ? "Success" : "Error"] = r.IsSuccessStatusCode
                ? "Successfully enrolled!"
                : await r.Content.ReadAsStringAsync();
            return RedirectToAction("Details", new { id = eventId });
        }

        // ---------- MY ENROLMENTS ----------
        public async Task<IActionResult> MyEnrolments()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync("api/enrolments/mine");
            var json = await r.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<MyEnrolmentVm>>(json, Opts) ?? new();
            return View(list);
        }

        // ---------- MY RESULTS ----------
        public async Task<IActionResult> MyResults()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync("api/results/mine");
            var json = await r.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<MyResultVm>>(json, Opts) ?? new();
            return View(list);
        }

        // ---------- PROFILE ----------
        public async Task<IActionResult> Profile()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync("api/users/me");
            var json = await r.Content.ReadAsStringAsync();
            var me = JsonSerializer.Deserialize<MeResponse>(json, Opts)!;
            return View(me);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(MeResponse model)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.PutAsync("api/users/me", new
            {
                fullName = model.FullName,
                phoneNumber = model.PhoneNumber
            });

            TempData[r.IsSuccessStatusCode ? "Success" : "Error"] =
                r.IsSuccessStatusCode ? "Profile updated." : "Update failed.";
            return RedirectToAction("Profile");
        }
    }
}