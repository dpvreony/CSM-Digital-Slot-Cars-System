using SlotCarsGo.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SlotCarsGo.Models.Racing
{
    public class DriverResult
    {
        int sessionId;
        Driver driver;
        int playerNumber;
        int position;
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


        public DriverResult(Driver driver, int playerNumber)
        {
            this.Driver = driver;
            this.PlayerNumber = playerNumber;
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

        public Symbol Symbol { get => symbol; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public string HashIdentIcon
        {
            get { return GetHashCode().ToString() + "-icon"; }
        }

        public string HashIdentTitle
        {
            get { return GetHashCode().ToString() + "-title"; }
        }

        public override string ToString()
        {
            return $"{Driver.ControllerId} {Driver.Nickname}";
        }

        public SolidColorBrush SymbolBrush { get => symbolBrush; set => symbolBrush = value; }
        public int SessionId { get => sessionId; set => sessionId = value; }
        public int PlayerNumber { get => playerNumber; set => playerNumber = value; }
    }
}
