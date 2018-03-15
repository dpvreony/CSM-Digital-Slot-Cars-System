using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class BestLapTime
    {
        public string Id { get; set; }

        // Foreign key
        public string LapTimeId { get; set; }
        // Navigation property
        public LapTime LapTime { get; set; }

        // Foreign key
        public string ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign key
        public string CarId { get; set; }
        // Navigation property
        public Car Car { get; set; }
    }
}