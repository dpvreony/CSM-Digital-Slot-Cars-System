using System;
using System.Collections.Generic;
using System.Linq;

namespace SlotCarsGo_Server.Models.DTO
{
    public class LapTimeDTO
    {
        public int LapNumber { get; set; }
        public TimeSpan Time { get; set; }
        public string DriverResultId { get; set; }
    }
}
