using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class LapTime
    {
        public int Id { get; set; }

        public int LapNumber { get; set; }
        public TimeSpan Time { get; set; }

        // Foreign Key
        public int DriverId { get; set; }
        // Navigation property
        public Driver Driver { get; set; }

        // Foreign Key
        public int RaceSessionId { get; set; }
        // Navigation property
        public RaceSession RaceSession { get; set; }
    }
}