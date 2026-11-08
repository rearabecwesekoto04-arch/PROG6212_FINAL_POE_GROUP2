using RaceDayAPI.Models;

namespace RaceDayAPI.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            db.Database.EnsureCreated();          // also applies the Roles HasData
            if (db.Users.Any()) return;

            const string pw = "Password123!";     // demo only
            string Hash() => BCrypt.Net.BCrypt.HashPassword(pw);

            // ---------- USERS ----------
            var org1 = new User { RoleId = 1, FullName = "Tlhologelo Sekoto", Email = "tr.otokes23@raceday.co.za", PhoneNumber = "0821234567", PasswordHash = Hash() };
            var org2 = new User { RoleId = 1, FullName = "Thandi Mathe", Email = "sebotse.mm21@raceday.co.za", PhoneNumber = "0837654321", PasswordHash = Hash() };
            var par1 = new User { RoleId = 2, FullName = "Shonisani Thavhiwa", Email = "shoni.sp12@raceday.co.za", PhoneNumber = "0841112222", PasswordHash = Hash() };
            var par2 = new User { RoleId = 2, FullName = "Rearabecwe Tlatsi", Email = "tlatsi.otokes04@raceday.co.za", PhoneNumber = "0829998888", PasswordHash = Hash() };

            db.Users.AddRange(org1, org2, par1, par2);
            db.SaveChanges();

            // ---------- EVENTS + CATEGORIES ----------
            var joburg10k = new Event
            {
                Organiser = org1,
                EventName = "Joburg City 10K",
                Description = "A fast, flat road run through the heart of Johannesburg. Great for first-timers and seasoned runners alike.",
                EventDate = new DateTime(2026, 12, 12, 6, 0, 0),
                Location = "Johannesburg CBD, Gauteng",
                Distance = 10m,
                EventType = "Run",
                RouteInfo = "Start and finish at Mary Fitzgerald Square. Water points every 2.5 km.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "10K Open", CategoryType = "Distance", DistanceKm = 10m, Price = 150m, MaxParticipants = 500 },
                    new Category { CategoryName = "5K Fun Run", CategoryType = "Distance", DistanceKm = 5m, Price = 90m, MaxParticipants = 300 },
                    new Category { CategoryName = "Under 18", CategoryType = "Age", MinAge = 12, MaxAge = 17, Price = 60m, MaxParticipants = 100 }
                }
            };

            var trailRun = new Event
            {
                Organiser = org1,
                EventName = "Magaliesberg Trail Run",
                Description = "A scenic off-road challenge with rolling hills, river crossings and mountain views.",
                EventDate = new DateTime(2027, 1, 23, 6, 30, 0),
                Location = "Magaliesburg, Gauteng",
                Distance = 21m,
                EventType = "Run",
                RouteInfo = "Mixed single-track and farm roads. Trail shoes recommended.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "Half Marathon", CategoryType = "Distance", DistanceKm = 21m, Price = 280m, MaxParticipants = 250 },
                    new Category { CategoryName = "10K Trail", CategoryType = "Distance", DistanceKm = 10m, Price = 180m, MaxParticipants = 250 }
                }
            };

            var sowetoWalk = new Event
            {
                Organiser = org1,
                EventName = "Soweto Family Walk",
                Description = "A relaxed community walk for the whole family. Strollers and dogs on leashes welcome.",
                EventDate = new DateTime(2026, 12, 5, 8, 0, 0),
                Location = "Orlando Stadium, Soweto",
                Distance = 5m,
                EventType = "Walk",
                RouteInfo = "Loop around Orlando West with a stop at Vilakazi Street.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "5K Walk", CategoryType = "Distance", DistanceKm = 5m, Price = 50m, MaxParticipants = 400 },
                    new Category { CategoryName = "Seniors 60+", CategoryType = "Age", MinAge = 60, MaxAge = 99, Price = 30m, MaxParticipants = 100 }
                }
            };

            var pretoriaCycle = new Event
            {
                Organiser = org2,
                EventName = "Pretoria Sunrise Cycle",
                Description = "An early-morning road cycle through the jacaranda-lined streets of Pretoria.",
                EventDate = new DateTime(2026, 12, 19, 5, 30, 0),
                Location = "Union Buildings, Pretoria",
                Distance = 40m,
                EventType = "Cycle",
                RouteInfo = "Closed roads. Helmets are compulsory.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "40K Road Ride", CategoryType = "Distance", DistanceKm = 40m, Price = 220m, MaxParticipants = 300 },
                    new Category { CategoryName = "20K Social Ride", CategoryType = "Distance", DistanceKm = 20m, Price = 140m, MaxParticipants = 300 },
                    new Category { CategoryName = "Juniors", CategoryType = "Age", MinAge = 10, MaxAge = 15, Price = 80m, MaxParticipants = 80 }
                }
            };

            var sandtonNight = new Event
            {
                Organiser = org2,
                EventName = "Sandton Night Run",
                Description = "Run through Sandton after dark under the city lights. Headlamps provided at registration.",
                EventDate = new DateTime(2027, 2, 6, 18, 30, 0),
                Location = "Nelson Mandela Square, Sandton",
                Distance = 8m,
                EventType = "Run",
                RouteInfo = "Well-lit loop route through Sandton CBD.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "8K Night Run", CategoryType = "Distance", DistanceKm = 8m, Price = 160m, MaxParticipants = 400 },
                    new Category { CategoryName = "Masters 40+", CategoryType = "Age", MinAge = 40, MaxAge = 59, Price = 160m, MaxParticipants = 150 }
                }
            };

            var cradle15k = new Event
            {
                Organiser = org2,
                EventName = "Cradle Valley 15K",
                Description = "A scenic country run through the Cradle of Humankind, finishing with a community braai.",
                EventDate = new DateTime(2027, 3, 13, 7, 0, 0),
                Location = "Cradle of Humankind, Gauteng",
                Distance = 15m,
                EventType = "Run",
                RouteInfo = "Gravel and tar mix. Hydration stations every 3 km.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "15K Open", CategoryType = "Distance", DistanceKm = 15m, Price = 200m, MaxParticipants = 350 },
                    new Category { CategoryName = "5K Family", CategoryType = "Distance", DistanceKm = 5m, Price = 70m, MaxParticipants = 200 }
                }
            };

            // A past event, so there are results to look at
            var springClassic = new Event
            {
                Organiser = org1,
                EventName = "Spring Classic",
                Description = "Our annual spring road race through the northern suburbs. Results are in!",
                EventDate = new DateTime(2026, 10, 17, 6, 30, 0),
                Location = "Northcliff, Johannesburg",
                Distance = 10m,
                EventType = "Run",
                RouteInfo = "Out-and-back route along Beyers Naude Drive.",
                Categories = new List<Category>
                {
                    new Category { CategoryName = "5K Open", CategoryType = "Distance", DistanceKm = 5m, Price = 100m, MaxParticipants = 300 },
                    new Category { CategoryName = "10K Open", CategoryType = "Distance", DistanceKm = 10m, Price = 140m, MaxParticipants = 300 }
                }
            };

            db.Events.AddRange(joburg10k, trailRun, sowetoWalk, pretoriaCycle, sandtonNight, cradle15k, springClassic);
            db.SaveChanges();

            // ---------- ENROLMENTS (2 per event, one from each participant) ----------
            // Helper: builds one enrolment
            Enrolment Enrol(User participant, Category category, DateTime date, string status = "Confirmed")
                => new Enrolment
                {
                    Participant = participant,
                    Category = category,
                    EnrolmentDate = date,
                    Status = status
                };

            db.AddRange(
                // Joburg City 10K
                Enrol(par1, joburg10k.Categories[0], new DateTime(2026, 11, 2)),               // 10K Open
                Enrol(par2, joburg10k.Categories[1], new DateTime(2026, 11, 3)),               // 5K Fun Run

                // Magaliesberg Trail Run
                Enrol(par1, trailRun.Categories[0], new DateTime(2026, 11, 4)),                // Half Marathon
                Enrol(par2, trailRun.Categories[1], new DateTime(2026, 11, 4), "Pending"),     // 10K Trail

                // Soweto Family Walk
                Enrol(par1, sowetoWalk.Categories[0], new DateTime(2026, 11, 1)),              // 5K Walk
                Enrol(par2, sowetoWalk.Categories[0], new DateTime(2026, 11, 1)),              // 5K Walk

                // Pretoria Sunrise Cycle
                Enrol(par1, pretoriaCycle.Categories[0], new DateTime(2026, 11, 6)),           // 40K Road Ride
                Enrol(par2, pretoriaCycle.Categories[1], new DateTime(2026, 11, 6), "Pending"),// 20K Social Ride

                // Sandton Night Run
                Enrol(par1, sandtonNight.Categories[0], new DateTime(2026, 11, 5), "Pending"), // 8K Night Run
                Enrol(par2, sandtonNight.Categories[0], new DateTime(2026, 11, 7)),            // 8K Night Run

                // Cradle Valley 15K
                Enrol(par1, cradle15k.Categories[0], new DateTime(2026, 11, 7)),               // 15K Open
                Enrol(par2, cradle15k.Categories[1], new DateTime(2026, 11, 8))                // 5K Family
            );

            // Spring Classic (past event) – enrolments WITH results
            db.Add(new Enrolment
            {
                Participant = par1,
                Category = springClassic.Categories[0],  // 5K Open
                EnrolmentDate = new DateTime(2026, 9, 28),
                Status = "Confirmed",
                Result = new Result
                {
                    FinishTime = new TimeSpan(0, 24, 35),
                    Position = 12,
                    Status = "Finished"
                }
            });

            db.Add(new Enrolment
            {
                Participant = par2,
                Category = springClassic.Categories[1],  // 10K Open
                EnrolmentDate = new DateTime(2026, 9, 30),
                Status = "Confirmed",
                Result = new Result
                {
                    FinishTime = null,
                    Position = null,
                    Status = "DNF"
                }
            });

            db.SaveChanges();
        }
    }
}