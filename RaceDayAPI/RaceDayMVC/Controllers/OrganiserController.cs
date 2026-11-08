using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Helpers;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Net;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiClient _api;
        private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

        public OrganiserController(ApiClient api) => _api = api;

        private bool Guard() => AuthCheck.IsOrganiser(HttpContext.Session);
        private int Me => AuthCheck.CurrentUserId(HttpContext.Session);

        // ---------- DASHBOARD ----------
        public async Task<IActionResult> Index()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            // 1. Get all events, filter by this organiser
            var evR = await _api.GetAsync("api/events");
            var evJson = await evR.Content.ReadAsStringAsync();
            var all = JsonSerializer.Deserialize<List<EventVm>>(evJson, Opts) ?? new();
            var mine = all.Where(e => e.OrganiserId == Me).ToList();

            // 2. Count enrolments per event
            var counts = new Dictionary<int, int>();
            foreach (var ev in mine)
            {
                var enR = await _api.GetAsync($"api/enrolments/event/{ev.EventId}");
                var enJson = await enR.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<EventEnrolmentVm>>(enJson, Opts);
                counts[ev.EventId] = list?.Count ?? 0;
            }

            return View(new DashboardVm { Events = mine, EnrolmentCounts = counts });
        }

        // ---------- CREATE EVENT ----------
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!Guard()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventVm model)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.PostAsync("api/events", new
            {
                eventName = model.EventName,
                description = model.Description,
                eventDate = model.EventDate,
                location = model.Location,
                distance = model.Distance,
                eventType = model.EventType,
                routeInfo = model.RouteInfo
            });

            if (!r.IsSuccessStatusCode)
            {
                ViewBag.Error = $"API {(int)r.StatusCode}: {await r.Content.ReadAsStringAsync()}";
                return View(model);
            }

            var json = await r.Content.ReadAsStringAsync();
            var created = JsonSerializer.Deserialize<EventVm>(json, Opts)!;

            if (model.BannerImage != null && model.BannerImage.Length > 0)
                await _api.PostMultipartAsync($"api/events/{created.EventId}/banner", model.BannerImage, "file");

            TempData["Success"] = "Event created!";
            return RedirectToAction("Index");
        }

        // ---------- EDIT EVENT ----------
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync($"api/events/{id}");
            if (!r.IsSuccessStatusCode) return NotFound();
            var json = await r.Content.ReadAsStringAsync();
            var ev = JsonSerializer.Deserialize<EventVm>(json, Opts)!;
            if (ev.OrganiserId != Me) return Forbid();
            return View(ev);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(int id, EventVm model, IFormFile? bannerImage)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.PutAsync($"api/events/{id}", new
            {
                eventName = model.EventName,
                description = model.Description,
                eventDate = model.EventDate,
                location = model.Location,
                distance = model.Distance,
                eventType = model.EventType,
                routeInfo = model.RouteInfo
            });

            if (!r.IsSuccessStatusCode)
            {
                ViewBag.Error = await r.Content.ReadAsStringAsync();
                return View(model);
            }

            if (bannerImage != null && bannerImage.Length > 0)
                await _api.PostMultipartAsync($"api/events/{id}/banner", bannerImage, "file");

            TempData["Success"] = "Event updated.";
            return RedirectToAction("Index");
        }

        // ---------- DELETE EVENT ----------
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            await _api.DeleteAsync($"api/events/{id}");
            TempData["Success"] = "Event deleted.";
            return RedirectToAction("Index");
        }

        // ---------- CATEGORIES ----------
        public async Task<IActionResult> Categories(int eventId)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync($"api/categories?eventId={eventId}");
            var json = await r.Content.ReadAsStringAsync();
            var cats = JsonSerializer.Deserialize<List<CategoryVm>>(json, Opts) ?? new();
            ViewBag.EventId = eventId;
            return View(cats);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(int eventId, CategoryVm model)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.PostAsync("api/categories", new
            {
                eventId,
                categoryName = model.CategoryName,
                categoryType = model.CategoryType,
                distanceKm = model.DistanceKm,
                minAge = model.MinAge,
                maxAge = model.MaxAge,
                price = model.Price,
                maxParticipants = model.MaxParticipants
            });

            TempData[r.IsSuccessStatusCode ? "Success" : "Error"] =
                r.IsSuccessStatusCode ? "Category added." : await r.Content.ReadAsStringAsync();
            return RedirectToAction("Categories", new { eventId });
        }

        // ---------- ENROLMENTS ----------
        public async Task<IActionResult> Enrolments(int eventId)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var r = await _api.GetAsync($"api/enrolments/event/{eventId}");
            var json = await r.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<EventEnrolmentVm>>(json, Opts) ?? new();
            ViewBag.EventId = eventId;
            return View(list);
        }

        // ---------- CAPTURE RESULTS ----------
        public async Task<IActionResult> CaptureResults(int eventId)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var enR = await _api.GetAsync($"api/enrolments/event/{eventId}");
            var enJson = await enR.Content.ReadAsStringAsync();
            var enrolments = JsonSerializer.Deserialize<List<EventEnrolmentVm>>(enJson, Opts) ?? new();

            var resR = await _api.GetAsync($"api/results/event/{eventId}");
            var resJson = await resR.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<List<EventResultVm>>(resJson, Opts) ?? new();

            ViewBag.EventId = eventId;
            ViewBag.Results = results;
            return View(enrolments);
        }

        [HttpPost]
        public async Task<IActionResult> CaptureResult(int enrolmentId, int eventId, string finishTime, int? position, string status)
        {
            if (!Guard()) return RedirectToAction("Login", "Account");

            var payload = new { enrolmentId, finishTime, position, status };
            var r = await _api.PostAsync("api/results", payload);

            // If result already exists, update it
            if (r.StatusCode == HttpStatusCode.Conflict)
            {
                var resR = await _api.GetAsync($"api/results/event/{eventId}");
                var resJson = await resR.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<EventResultVm>>(resJson, Opts) ?? new();
                var existing = list.FirstOrDefault(x => x.EnrolmentId == enrolmentId);
                if (existing != null)
                    r = await _api.PutAsync($"api/results/{existing.ResultId}", payload);
            }

            TempData[r.IsSuccessStatusCode ? "Success" : "Error"] =
                r.IsSuccessStatusCode ? "Result saved." : await r.Content.ReadAsStringAsync();
            return RedirectToAction("CaptureResults", new { eventId });
        }
    }
}