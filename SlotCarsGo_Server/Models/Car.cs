using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Car
    {
        public Car()
        {
            this.ImageName = "0.jpg";
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageName { get; set; }

        public bool Selectable { get; set; } = true;

        // Foreign key
        [Required]
        public string TrackId { get; set; }
        // Navigation property
        public Track Track { get; set; }

        // Foreign Key
        public string BestLapTimeId { get; set; }
        // Navigation Property
        public BestLapTime BestLapTime { get; set; }

        public virtual ICollection<BestLapTime> BestLapTimes { get; set; }
    }
}