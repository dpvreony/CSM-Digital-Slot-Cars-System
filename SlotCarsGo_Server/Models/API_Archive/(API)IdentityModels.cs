using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace SlotCarsGo_Server.Models.API_Archive
{
    // You can add profile data for the user by adding more properties to your ApiApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApiApplicationUser : IdentityUser
    {
        public string ImageName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApiApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApiApplicationDbContext : IdentityDbContext<ApiApplicationUser>
    {
        public ApiApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApiApplicationDbContext Create()
        {
            return new ApiApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.RaceSession> RaceSessions { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.Car> Cars { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.Driver> Drivers { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.DriverResult> DriverResults { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.Track> Tracks { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.RaceType> RaceTypes { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.LapTime> LapTimes { get; set; }

        public System.Data.Entity.DbSet<SlotCarsGo_Server.Models.API_Archive.ApiApplicationUser> ApiApplicationUsers { get; set; }
    }
}