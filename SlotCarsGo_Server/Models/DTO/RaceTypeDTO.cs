using System;

namespace SlotCarsGo_Server.Models.DTO
{
    public class RaceTypeDTO
    {
        public int RaceTypeId { get; set;}
        public string Name { get; set; }
        public string Rules { get; set; }
        public int RaceLimitValue { get; set; }
        public TimeSpan RaceLength { get; set; }
        public bool LapsNotDuration { get; set; }
        public bool FuelEnabled { get; set; }
        public int CrashPenalty { get; set;}
        public string Symbol { get; set; }
    }
}
