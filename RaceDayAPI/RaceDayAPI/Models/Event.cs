namespace RaceDayAPI.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int OrganiserId { get; set; }
        public User Organiser { get; set; } = null!;
        public string EventName { get; set; } = "";
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = "";
        public decimal Distance { get; set; }
        public string EventType { get; set; } = "";
        public string? RouteInfo { get; set; }
        public string? BannerUrl { get; set; } // ← ADDED
        public List<Category> Categories { get; set; } = new();
    }
}