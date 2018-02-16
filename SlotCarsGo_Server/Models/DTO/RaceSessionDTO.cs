using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SlotCarsGo_Server.Models.DTO
{
    public class RaceSessionDTO
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public RaceTypeDTO RaceType;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfDrivers { get; set; }
    }
}
