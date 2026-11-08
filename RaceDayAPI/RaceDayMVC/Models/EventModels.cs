using Microsoft.AspNetCore.Http;

namespace RaceDayMVC.Models
{
    public class EventVm
    {
        public int EventId { get; set; }
        public int OrganiserId { get; set; }
        public string EventName { get; set; } = "";
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = "";
        public decimal Distance { get; set; }
        public string EventType { get; set; } = "";
        public string? RouteInfo { get; set; }
        public string? BannerUrl { get; set; }
    }

    public class CategoryVm
    {
        public int CategoryId { get; set; }
        public int EventId { get; set; }
        public string CategoryName { get; set; } = "";
        public string CategoryType { get; set; } = "";
        public decimal? DistanceKm { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public decimal Price { get; set; }
        public int? MaxParticipants { get; set; }
    }

    public class MyEnrolmentVm
    {
        public int EnrolmentId { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; } = "";
        public string Category { get; set; } = "";
        public int EventId { get; set; }
        public string Event { get; set; } = "";
    }

    public class MyResultVm
    {
        public int ResultId { get; set; }
        public TimeSpan? FinishTime { get; set; }
        public int? Position { get; set; }
        public string Status { get; set; } = "";
        public string Event { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class EventEnrolmentVm
    {
        public int EnrolmentId { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; } = "";
        public string Participant { get; set; } = "";
        public string ParticipantEmail { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class EventResultVm
    {
        public int ResultId { get; set; }
        public int EnrolmentId { get; set; }
        public TimeSpan? FinishTime { get; set; }
        public int? Position { get; set; }
        public string Status { get; set; } = "";
        public string Participant { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class DashboardVm
    {
        public List<EventVm> Events { get; set; } = new();
        public Dictionary<int, int> EnrolmentCounts { get; set; } = new();
    }

    public class CreateEventVm
    {
        public string EventName { get; set; } = "";
        public string? Description { get; set; }
        public DateTime EventDate { get; set; } = DateTime.Today.AddMonths(1);
        public string Location { get; set; } = "";
        public decimal Distance { get; set; }
        public string EventType { get; set; } = "Run";
        public string? RouteInfo { get; set; }
        public IFormFile? BannerImage { get; set; }
    }
}