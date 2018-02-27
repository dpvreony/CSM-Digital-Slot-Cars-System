using System;

namespace SlotCarsGo_Server.Models.DTO
{
    public class CarDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TrackRecord { get; set; }
        public string RecordHolder { get; set; }
        public string ImageName { get; set; }
    }
}
