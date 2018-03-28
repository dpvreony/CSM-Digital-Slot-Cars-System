using System;

namespace SlotCarsGo_Server.Models.DTO
{
    public class TrackDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Length { get; set; }
        public string Secret { get; set; }
        public TimeSpan TrackRecord { get; set; }
        public string RecordHolder { get; set; }
        public string OwnerEmail { get; set; }
    }
}
