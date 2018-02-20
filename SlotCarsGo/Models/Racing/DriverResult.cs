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
        int raceSessionId;
        Driver driver;
        int controllerNumber;
        int position;
        int carId;
        Car car;
        int laps;
        private bool finished;
        private float fuel;
        TimeSpan totalTime;
        TimeSpan bestLap;
        TimeSpan timeOffPace;
        private List<TimeSpan> lapTimes;
        private TimeSpan previousLapTime;
        private TimeSpan bestLapTime;
        private List<UInt32> lapGameCounters;
        private UInt32 previousGameCounter;
        private Symbol symbol = Symbol.Flag;
        private SolidColorBrush symbolBrush;

        public DriverResult(Driver driver, int controllerNumber)
        {
            this.Driver = driver;
            this.ControllerNumber = controllerNumber;
            this.Car = driver.SelectedCar;
            this.Position = 0;
            this.Laps = 0;
            this.TotalTime = new TimeSpan();
            this.BestLap = new TimeSpan();
            this.TimeOffPace = new TimeSpan();
            this.LapTimes = new List<TimeSpan>();
            this.PreviousLapTime = new TimeSpan();
            this.BestLapTime = new TimeSpan();
            this.LapGameCounters = new List<UInt32>();
            this.PreviousGameCounter = 0;
        }

        public Driver Driver { get => driver; private set => driver = value; }
        public int Position { get => position; set => this.position = value; }
        public int RaceSessionId { get => raceSessionId; set => raceSessionId = value; }
        public int ControllerNumber { get => controllerNumber; set => controllerNumber = value; }
        public int CarId { get => carId; set => carId = value; }
        public Car Car { get => car; private set => car = value; }
        public int Laps { get => laps; set => laps = value; }
        public bool Finished { get => finished; set => finished = value; }
        public float Fuel { get => fuel; set => fuel = value; }
        public TimeSpan TotalTime { get => totalTime; set => totalTime = value; }
        public TimeSpan BestLap { get => bestLap; set => bestLap = value; }
        public TimeSpan TimeOffPace { get => timeOffPace; set => timeOffPace = value; }
        public List<TimeSpan> LapTimes { get => lapTimes; set => lapTimes = value; }
        public TimeSpan PreviousLapTime { get => previousLapTime; set => previousLapTime = value; }
        public TimeSpan BestLapTime { get => bestLapTime; set => bestLapTime = value; }
        public List<uint> LapGameCounters { get => lapGameCounters; set => lapGameCounters = value; }
        public uint PreviousGameCounter { get => previousGameCounter; set => previousGameCounter = value; }


        public override string ToString()
        {
            return $"{Driver.ControllerId} {Driver.UserName}";
        }
    }
}
