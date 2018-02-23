using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public TimeSpan TrackRecord { get; set; }
        public string ImageName { get; set; } = "0.png";

        // Foreign key
        public int? ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign key
        public int? TrackId { get; set; }
        // Navigation property
        public Track Track { get; set; }
    }
}