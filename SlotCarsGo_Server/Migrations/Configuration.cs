namespace SlotCarsGo_Server.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SlotCarsGo_Server.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SlotCarsGo_Server.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SlotCarsGo_Server.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            string defaultUserId = "1";
            Track defaultTrack;
            Car defaultCar;

            // User
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "Default User"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "Default User" };

                manager.Create(user, "DefaultUser1!");
                manager.AddToRole(user.Id, "AppAdmin");
                defaultUserId = user.Id;
            }

            // Track
            context.Tracks.AddOrUpdate(t => t.Id,
                new Track { ApplicationUserId = defaultUserId, Name = "SlotCarsGo Track", Length = 2.5f });

            defaultTrack = context.Tracks.Where(t => t.ApplicationUserId == defaultUserId) as Track;


            // Cars
            context.Cars.AddOrUpdate(c => c.Id,
                new Car { Name = "Ferrari F50",
                    TrackRecord =  new TimeSpan(0,0,30),
                    ImageName = "1.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                },
                new Car
                {
                    Name = "Bentley Continental GT3",
                    TrackRecord = new TimeSpan(0, 0, 30),
                    ImageName = "2.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                },
                new Car
                {
                    Name = "Ford Escort 1980 MKII",
                    TrackRecord = new TimeSpan(0, 0, 30),
                    ImageName = "3.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                },
                new Car
                {
                    Name = "Lancia Delta S4",
                    TrackRecord = new TimeSpan(0, 0, 30),
                    ImageName = "4.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                },
                new Car
                {
                    Name = "Volkswagon Polo WRC 2013",
                    TrackRecord = new TimeSpan(0, 0, 30),
                    ImageName = "5.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                },
                new Car
                {
                    Name = "Mini Countryman WRC 2012",
                    TrackRecord = new TimeSpan(0, 0, 30),
                    ImageName = "6.png",
                    ApplicationUserId = defaultUserId,
                    TrackId = defaultTrack.Id
                }
            );

            defaultCar = context.Cars.Where(c => c.Name == "Ferrari F50") as Car;

            // Drivers
            context.Drivers.AddOrUpdate(d => d.Id,
                new Driver
                {
                    TrackId = defaultTrack.Id,
                    ControllerId = 1,
                    ApplicationUserId = defaultUserId,
                    CarId = defaultCar.Id
                }
            );

            // RaceTypes
            context.RaceTypes.AddOrUpdate(rt => rt.Id,
                new RaceType
                {
                    Name = "Free Play",
                    Rules = "Players drive for the fun of it - no limit and no rules!",
                    Symbol = "57602"
                },

                new RaceType
                {
                    Name = "Qualifying",
                    Rules = "Players race one at a time or all together to record the fastest lap in the session. The results can be used to decide the grid order of a Grand Prix race.",
                    Symbol = "57833"
                },
                new RaceType
                {
                    Name = "Grand Prix",
                    Rules = "Multiple players race against each other to complete the set number of laps in the fastest time or the most laps in the set number of minutes.",
                    Symbol = "57641"
                },
                new RaceType
                {
                    Name = "Time Trial",
                    Rules = "Players race one at a time in a series to complete the set number of laps in the shortest time.",
                    Symbol = "57805"
                }
            );



        }
    }
}
