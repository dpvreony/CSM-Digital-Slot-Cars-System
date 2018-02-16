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
        public string ImagePath { get; set; } = "0.png";

        // Foreign key
        public int ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
    }
}