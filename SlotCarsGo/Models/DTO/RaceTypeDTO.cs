using System;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo_Server.Models.DTO
{
    public class RaceTypeDTO
    {
        public string RaceTypeId { get; set;}
        public string Name { get; set; }
        public string Rules { get; set; }
        public int RaceLimitValue { get; set; }
        public TimeSpan RaceLength { get; set; }
        public bool LapsNotDuration { get; set; }
        public bool FuelEnabled { get; set; }
        public int CrashPenalty { get; set;}
        public string Symbol { get; set; }

        public override string ToString()
        {
            return $"{this.Name} Race";
        }
    }
}
