using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Car
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public TimeSpan TrackRecord { get; set; }
        public string ImageName { get; set; } = "0.jpg";

        // Foreign key
        public string ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign key
        public string TrackId { get; set; }
        // Navigation property
        public Track Track { get; set; }
    }
}