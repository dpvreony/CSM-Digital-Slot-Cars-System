using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Helpers;
using SlotCarsGo.Models.Comms;
using SlotCarsGo.Models.Manager;
using SlotCarsGo.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Models.Racing
{
    class RaceSession
    {
        const float GameTimerToMicroSecondsConstant = 6.4f;

        private string sessionName;
        private int trackID; // TODO: Retrieve from XML on startup and store above Powerbase? RaceManager class?
        private RaceTypeBase raceType;
        private List<Player> players;
        private DateTime startTime;
        private DateTime endTime;
        private int lapsRemaining;
        private TimeSpan timeRemaining;
        private int numberOfPlayers;
        private bool started;
        private bool finished;
        private UInt32 startingGameTimer;
        // TODO: fastest lap of session - what format, to include driver?
        private TimeSpan zeroLapTime;
        private List<TimeSpan>[] driversLapTimes;
        private TimeSpan[] driversPreviousLapTime;
        private TimeSpan[] driversFastestLapTimes;
        private List<UInt32>[] driversLapGameCounters;
        private UInt32[] driversPreviousGameCounter;
        private bool[] driversFinished;
        private bool fuelEnabled;
        private float fuelFullTankWeight; // TODO: will be settable as a % to calculate initial weight penalty
        private float fuelBurnRatePerLap; // TODO: will be decremented from playerFuel each lap / game Timer span
        private float[] playerFuel; // TODO calculate throttle value based on fuel level (needs to factor fuel effect)

        // New Race session GUI creates a configured race type and passses to session?

        /// <summary>
        /// Constructor for a race session.
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="raceType"></param>
        /// <param name="players"></param>
        public RaceSession(RaceTypeBase raceType, ObservableCollection<User> users)
        {
            this.TrackID = AppManager.Track.Id;
            this.RaceType = raceType;
            this.Players = new List<Player>();
            foreach (var user in users)
            {
                this.Players.Add(new Player(user));
            }
            this.NumberOfPlayers = this.Players.Count;
            this.StartTime = DateTime.Now;
            this.Started = false;
            this.Finished = false;
            this.FuelEnabled = raceType.FuelEnabled;
            this.zeroLapTime = new TimeSpan();
            this.DriversLapTimes = new List<TimeSpan>[] { new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>() };
            this.DriversPreviousLapTime = new TimeSpan[] { new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan() };
            this.DriversFastestLapTimes = new TimeSpan[] { new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan() };
            this.DriversPreviousGameCounter = new UInt32[] { 0, 0, 0, 0, 0, 0 };
            this.DriversLapGameCounters = new List<UInt32>[] { new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>() };
            this.DriversFinished = new bool[] { false, false, false, false, false, false };
            this.PlayerFuel = new float[] { 0, 0, 0, 0, 0, 0};
        }

        public int TrackID { get => this.trackID; set => this.trackID = value; }

        public RaceTypeBase RaceType { get => this.raceType; set => this.raceType = value; }

        internal List<Player> Players { get => this.players; set => this.players = value; }

        public DateTime StartTime { get => this.startTime; set => this.startTime = value; }

        public DateTime EndTime { get => this.endTime; set => this.endTime = value; }

        internal List<TimeSpan>[] DriversLapTimes { get => driversLapTimes; set => driversLapTimes = value; }

        internal TimeSpan[] DriversPreviousLapTime { get => driversPreviousLapTime; set => driversPreviousLapTime = value; }

        internal TimeSpan[] DriversFastestLapTimes { get => driversFastestLapTimes; set => driversFastestLapTimes = value; }

        internal List<uint>[] DriversLapGameCounters { get => driversLapGameCounters; set => driversLapGameCounters = value; }

        internal uint[] DriversPreviousGameCounter { get => driversPreviousGameCounter; set => driversPreviousGameCounter = value; }

        internal bool[] DriversFinished { get => driversFinished; set => driversFinished = value; }

        public float[] PlayerFuel { get => playerFuel; set => playerFuel = value; }

        public bool FuelEnabled { get => fuelEnabled; set => fuelEnabled = value; }

        public uint StartingGameTimer { get => startingGameTimer; set => startingGameTimer = value; }

        internal bool Started { get => started; set => started = value; }

        internal bool Finished { get => finished; set => finished = value; }

        public int NumberOfPlayers { get => numberOfPlayers; set => numberOfPlayers = value; }

        /// <summary>
        /// Starts a race session, which restarts the Powerbase comms and initiates data recording.
        /// </summary>
        public void RaceStart()
        {
            this.Started = true;
            this.StartTime = DateTime.Now;
            SimpleIoc.Default.GetInstance<Powerbase>().Run(this);
        }

        /// <summary>
        /// Finishes the race including stopping the comms to the Powerbase.
        /// </summary>
        public void RaceFinish()
        {
            this.Finished = true;
            this.EndTime = DateTime.Now;
            SimpleIoc.Default.GetInstance<Powerbase>().Stop();
            // TODO: check that data is saved before quitting
            // TODO: Navigate to results page?
        }

        /// <summary>
        /// Processes the finsh line data packet to record laptimes and monitor race progress.
        /// </summary>
        /// <param name="carId">The carId.</param>
        /// <param name="gameTimer">The game timer data.</param>
        internal void ProcessFinishLineData(int carId, UInt32 gameTimer)
        {
            // Add to Driver's game counters list
            this.DriversLapGameCounters[carId].Add(gameTimer);

            // Calculate last lap as a counter;
            UInt32 lastLapAsGameTimer = gameTimer - this.DriversPreviousGameCounter[carId];

            // Update driver's previous game counter
            this.DriversPreviousGameCounter[carId] = gameTimer;

            // Calculate last lap counter as a timespan
            TimeSpan lapTime = this.ConvertGameTimerToTimeSpan(lastLapAsGameTimer);

            // Add last lap time to driver's lap times list
            this.DriversLapTimes[carId].Add(lapTime);

            // Update driver's previous lap time
            this.DriversPreviousLapTime[carId] = lapTime;

            // Check if last lap time is this driver's fastest in this session
            if (lapTime < this.DriversFastestLapTimes[carId] || this.DriversFastestLapTimes[carId] == zeroLapTime)
            {
                this.DriversFastestLapTimes[carId] = lapTime;
            }

            // Notify the HUD View Model that session values updated.
            SimpleIoc.Default.GetInstance<RaceHUDViewModel>().UpdateLapTimes(carId);

            // Check for race end conditions
            if (this.RaceType.LapsNotDuration)
            {
                if (this.DriversLapTimes[carId].Count >= this.RaceType.RaceLimitValue)
                {
                    this.Finished = true;
                    this.EndTime = DateTime.Now;
                    this.DriversFinished[carId] = true;
                    this.RaceFinish();
                    // TODO: create a routine that drives all cars to finish line! (before closing PB) 
                }
            }
            else
            {
                TimeSpan raceDuration = this.ConvertGameTimerToTimeSpan(gameTimer);
                if (raceDuration >= this.RaceType.RaceLength)
                {
                    this.Finished = true;
                    this.DriversFinished[0] = true;
                    this.DriversFinished[1] = true;
                    this.DriversFinished[2] = true;
                    this.DriversFinished[3] = true;
                    this.DriversFinished[4] = true;
                    this.DriversFinished[5] = true;
                }
            }

            System.Diagnostics.Debug.WriteLine($"CarId {carId + 1} : {lapTime}");

            //TODO: decide when to kill Powerbase comms (EndRace())
            // And or throttle finished cars 
        }

        /// <summary>
        /// Sets the starting game timer for all driver's previous game counter value.
        /// </summary>
        /// <param name="gameTimer">The starting game timer.</param>
        internal void SetRaceStartGameTimer(UInt32 gameTimer)
        {
            this.StartingGameTimer = gameTimer;
            this.DriversPreviousGameCounter[0] = gameTimer;
            this.DriversPreviousGameCounter[1] = gameTimer;
            this.DriversPreviousGameCounter[2] = gameTimer;
            this.DriversPreviousGameCounter[3] = gameTimer;
            this.DriversPreviousGameCounter[4] = gameTimer;
            this.DriversPreviousGameCounter[5] = gameTimer;
            this.Started = true;
        }

        /// <summary>
        /// Converts the game timer 32 bit number from microseconds in to milliseconds 
        /// and returns as a TimeSpan object. 
        /// </summary>
        /// <param name="gameTimer"></param>
        /// <returns>The Game Timer as a TimeSpan</returns>
        internal TimeSpan ConvertGameTimerToTimeSpan(UInt32 gameTimer)
        {
            return new TimeSpan(0, 0, 0, 0, (int)(gameTimer * RaceSession.GameTimerToMicroSecondsConstant) / 1000);
        }
    }
}
