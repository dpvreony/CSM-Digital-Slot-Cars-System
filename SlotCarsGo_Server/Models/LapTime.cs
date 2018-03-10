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
        public string DriverResultId { get; set; }
        // Navigation property
        public DriverResult DriverResult { get; set; }
    }
}