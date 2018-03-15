using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Track
    {
        public Track()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
            this.Cars = new HashSet<Car>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public float Length { get; set; }

        public string Secret { get; set; }

        public string BestLapTimeId { get; set; }
        public BestLapTime BestLapTime { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}