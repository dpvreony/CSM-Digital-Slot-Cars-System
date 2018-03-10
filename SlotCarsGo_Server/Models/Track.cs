using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Key]
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        public string Name { get; set; }

        public float Length { get; set; }

        public string Secret { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}