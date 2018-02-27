using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models.DTO
{
    public class LapTimeDTO
    {
        public string Id { get; set; }
        public int LapNumber { get; set; }
        public TimeSpan Time { get; set; }
        public string DriverId { get; set; }
        public string RaceSessionId { get; set; }
    }
}