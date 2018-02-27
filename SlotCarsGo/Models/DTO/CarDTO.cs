using System;

namespace SlotCarsGo_Server.Models.DTO
{
    public class CarDTO
    {
        public string CarID { get; set; }
        public string Name { get; set; }
        public TimeSpan TrackRecord { get; set; }
        public String RecordHolder { get; set; }
        public string ImagePath { get; set; }
    }
}
