using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models.DTO
{
    public class LapTimeDTO
    {
        public int Id { get; set; }
        public int LapNumber { get; set; }
        public TimeSpan Time { get; set; }
        public int DriverId { get; set; }
        public int RaceSessionId { get; set; }
    }
}