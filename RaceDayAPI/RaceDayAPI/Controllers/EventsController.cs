using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Dtos;
using RaceDayAPI.Services;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [ApiController, Route("api/events"), Authorize]
    public class EventsController(AppDbContext db, IBlobService blobs) : ControllerBase
    {
        int CallerId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await db.Events.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ev = await db.Events.FindAsync(id);
            return ev == null ? NotFound() : Ok(ev);
        }

        [HttpPost, Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Create(Dtos.Dtos.EventDto d)
        {
            var ev = new Event
            {
                OrganiserId = CallerId,
                EventName = d.EventName,
                Description = d.Description,
                EventDate = d.EventDate,
                Location = d.Location,
                Distance = d.Distance,
                EventType = d.EventType,
                RouteInfo = d.RouteInfo
            };
            db.Events.Add(ev); await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = ev.EventId }, ev);
        }

        [HttpPut("{id}"), Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Update(int id, Dtos.Dtos.EventDto d)
        {
            var ev = await db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserId != CallerId) return Forbid();
            ev.EventName = d.EventName; ev.Description = d.Description; ev.EventDate = d.EventDate;
            ev.Location = d.Location; ev.Distance = d.Distance; ev.EventType = d.EventType;
            ev.RouteInfo = d.RouteInfo;
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Organiser")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserId != CallerId) return Forbid();
            db.Events.Remove(ev); await db.SaveChangesAsync();
            return NoContent();
        }

        // ─ NEW: BANNER UPLOAD ─
        [HttpPost("{id}/banner"), Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UploadBanner(int id, IFormFile file)
        {
            var ev = await db.Events.FindAsync(id);
            if (ev == null) return NotFound();
            if (ev.OrganiserId != CallerId) return Forbid();
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var name = $"event-{id}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            ev.BannerUrl = await blobs.UploadAsync(file, name);
            await db.SaveChangesAsync();
            return Ok(new { bannerUrl = ev.BannerUrl });
        }
    }
}