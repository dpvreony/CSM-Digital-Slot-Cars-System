using SlotCarsGo.Models.Manager;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SlotCarsGo.Models.Racing
{
    /// <summary>
    /// Represents a drivers race session result, suitable for displaying in the results
    /// View and for sending to Entity Framework in the server.
    /// </summary>
    public class DriverResult
    {
        public DriverResult()
        {
            this.Position = 0;
            this.Laps = 0;
            this.TotalTime = new TimeSpan();
            this.TimeOffPace = new TimeSpan();
            this.LapTimes = new List<TimeSpan>();
            this.PreviousLapTime = new TimeSpan();
            this.BestLapTime = new TimeSpan();
            this.LapGameCounters = new List<UInt32>();
            this.PreviousGameCounter = 0;
        }

        public Driver Driver { get; set; }
        public int Position { get; set; }
        public string RaceSessionId { get; set; }
        public int ControllerId { get; set; }
        public string CarId { get; set; }
        public Car Car { get; set; }
        public int Laps { get; set; }
        public bool Finished { get; set; }
        public float Fuel { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan TimeOffPace { get; set; }
        public List<TimeSpan> LapTimes { get; set; }
        public TimeSpan PreviousLapTime { get; set; }
        public TimeSpan BestLapTime { get; set; }
        public List<uint> LapGameCounters { get; set; }
        public uint PreviousGameCounter { get; set; }
        public Symbol Symbol = Symbol.Flag;
        public SolidColorBrush SymbolBrush { get; set; }


        public override string ToString()
        {
            return $"{Driver.ControllerId} {Driver.UserName}";
        }
    }
}
