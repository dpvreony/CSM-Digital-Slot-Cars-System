using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SlotCarsGo_Server.App_Start;
using System.Collections.Generic;

namespace SlotCarsGo_Server.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {

            this.Tracks = new HashSet<Track>();
            this.DriverResults = new HashSet<DriverResult>();
        }

        public string ImageName { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<DriverResult> DriverResults { get; set; }
        public virtual ICollection<BestLapTime> BestLapTimes { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<RaceSession> RaceSessions { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<DriverResult> DriverResults { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<RaceType> RaceTypes { get; set; }

        public DbSet<LapTime> LapTimes { get; set; }

        public DbSet<BestLapTime> BestLapTimes { get; set; }
    }
}