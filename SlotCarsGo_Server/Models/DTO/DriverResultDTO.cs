using System;
using System.Collections.Generic;

namespace SlotCarsGo_Server.Models.DTO
{
    /// <summary>
    /// Represents a drivers race session result, suitable for displaying in the results
    /// View and for sending to Entity Framework in the server.
    /// </summary>
    public class DriverResultDTO
    {
        public int DriverId { get; set; }
        public int Position { get; set; }
        public int RaceSessionId { get; set; }
        public int ControllerNumber { get; set; }
        public int CarId { get; set; }
        public int Laps { get; set; }
        public bool Finished { get; set; }
        public float Fuel { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan TimeOffPace { get; set; }
        public TimeSpan BestLapTime { get; set; }
    }
}
