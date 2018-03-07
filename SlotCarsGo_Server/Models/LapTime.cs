using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class LapTime
    {
        public string Id { get; set; }

        public int LapNumber { get; set; }
        public TimeSpan Time { get; set; }

        // Foreign Key
        public string DriverId { get; set; }
        // Navigation property
        public Driver Driver { get; set; }

        // Foreign Key
        public string RaceSessionId { get; set; }
        // Navigation property
        public RaceSession RaceSession { get; set; }
    }
}