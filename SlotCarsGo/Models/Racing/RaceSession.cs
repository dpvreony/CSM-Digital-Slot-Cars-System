using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Helpers;
using SlotCarsGo.Models.Comms;
using SlotCarsGo.Models.Manager;
using SlotCarsGo.ViewModels;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Models.Racing
{
    public class RaceSession
    {
        const float GameTimerToMicroSecondsConstant = 6.4f;

        private int sessionId;
        private string sessionName;
        private int trackID; // TODO: Retrieve from XML on startup and store above Powerbase? RaceManager class?
        private RaceTypeBase raceType;
        private List<Driver> drivers;
        private DateTime startTime;
        private DateTime endTime;
        private int lapsRemaining;
        private TimeSpan timeRemaining;
        private int numberOfDrivers;
        private bool started;
        private bool finished;
        private int finishPosition = 1;
        private UInt32 startingGameTimer;
        // TODO: fastest lap of session - what format, to include driver?
        private TimeSpan zeroLapTime = new TimeSpan(0);
        private Dictionary<int, DriverResult> driverResults;
        private bool[] driversFinished;
        private TimeSpan winningTime;
        private bool fuelEnabled;
        private float fuelFullTankWeight; // TODO: will be settable as a % to calculate initial weight penalty
        private float fuelBurnRatePerLap; // TODO: will be decremented from playerFuel each lap / game Timer span
        private float[] playerFuel; // TODO calculate throttle value based on fuel level (needs to factor fuel effect)

        /// <summary>
        /// Constructor for a race session.
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="raceType"></param>
        /// <param name="players"></param>
        public RaceSession(RaceTypeBase raceType, ObservableCollection<Driver> users)
        {
            this.SessionId = 0; // Updated at end of race
            this.TrackID = AppManager.Track.Id;
            this.RaceType = raceType;
            this.FuelEnabled = raceType.FuelEnabled;
            this.Drivers = users.ToList();
            this.NumberOfDrivers = this.Drivers.Count;

            this.InitialiseSession();
        }

        public int TrackID { get => this.trackID; set => this.trackID = value; }
        public RaceTypeBase RaceType { get => this.raceType; set => this.raceType = value; }
        public DateTime StartTime { get => this.startTime; set => this.startTime = value; }
        public DateTime EndTime { get => this.endTime; set => this.endTime = value; }
        public bool FuelEnabled { get => fuelEnabled; set => fuelEnabled = value; }
        public uint StartingGameTimer { get => startingGameTimer; set => startingGameTimer = value; }
        internal bool Started { get => started; set => started = value; }
        internal bool Finished { get => finished; set => finished = value; }
        public int NumberOfDrivers { get => numberOfDrivers; set => numberOfDrivers = value; }
        public int LapsRemaining { get => lapsRemaining; set => lapsRemaining = value; }
        public List<Driver> Drivers { get => drivers; set => drivers = value; }
        internal Dictionary<int, DriverResult> DriverResults { get => driverResults; }
        public int SessionId { get => sessionId; set => sessionId = value; }

        /// <summary>
        /// Initialises the data driven session details.
        /// </summary>
        private void InitialiseSession()
        {
            this.StartTime = DateTime.Now;
            this.Started = false;
            this.Finished = false;
            this.driverResults = new Dictionary<int, DriverResult>();
            int playerNumber = 1;
            foreach (Driver driver in this.Drivers)
            {
                this.driverResults.Add(driver.ControllerId,  new DriverResult(driver, playerNumber++));
            }
            this.LapsRemaining = this.RaceType.LapsNotDuration ? this.RaceType.RaceLimitValue : 0;
        }

        /// <summary>
        /// Starts a race session, which restarts the Powerbase comms and initiates data recording.
        /// </summary>
        public void StartRace()
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString()}: Session start race..");
            this.Started = true;
            this.StartTime = DateTime.Now;
        }

        /// <summary>
        /// Finishes the race.
        /// </summary>
        public void FinishRace()
        {
            this.Finished = true;
            this.EndTime = DateTime.Now;
        }

        /// <summary>
        /// Finishes race and stops the powerbase listening loop.
        /// </summary>
        public void QuitRace()
        {
            this.Started = false;
            this.Finished = true;
        }

        /// <summary>
        /// Resets this session data and stops the powerbase listening loop.
        /// </summary>
        public void ResetRace()
        {
            this.InitialiseSession();
        }

        /// <summary>
        /// Processes the finsh line data packet to record laptimes and monitor race progress.
        /// </summary>
        /// <param name="carId">The carId.</param>
        /// <param name="gameTimer">The game timer data.</param>
        internal void ProcessFinishLineData(int carId, UInt32 gameTimer)
        {
            if (this.DriverResults.ContainsKey(carId))
            {
                // Calculate last lap as a counter;
                UInt32 lastLapAsGameTimer = gameTimer - this.driverResults[carId].PreviousGameCounter;

                // Calculate last lap counter as a timespan
                TimeSpan lapTime = this.ConvertGameTimerToTimeSpan(lastLapAsGameTimer);

                if (lapTime > TimeSpan.FromMilliseconds(750))
                {
                    if (!this.driverResults[carId].Finished)
                    {
                        // Calculate current race duration
                        TimeSpan raceDuration = this.ConvertGameTimerToTimeSpan(gameTimer);

                        // Increment laps
                        this.driverResults[carId].Laps += 1;

                        // Add to Driver's game counters list
                        this.driverResults[carId].LapGameCounters.Add(gameTimer);

                        // Update driver's previous game counter
                        this.driverResults[carId].PreviousGameCounter = gameTimer;

                        // Add last lap time to driver's lap times list
                        this.driverResults[carId].LapTimes.Add(lapTime);

                        // Update driver's previous lap time
                        this.driverResults[carId].PreviousLapTime = lapTime;

                        // Check if last lap time is this driver's fastest in this session
                        if (lapTime < this.driverResults[carId].BestLapTime || this.driverResults[carId].BestLapTime == zeroLapTime)
                        {
                            this.driverResults[carId].BestLapTime = lapTime;
                        }

                        // Notify the HUD View Model that session values updated.
                        SimpleIoc.Default.GetInstance<RaceHUDViewModel>().UpdateLapTimes(carId);

                        // Check for race end conditions
                        if (this.RaceType.LapsNotDuration)
                        {
                            int thisCarLapsRemaining = this.RaceType.RaceLimitValue - this.driverResults[carId].Laps;
                            this.LapsRemaining = thisCarLapsRemaining < this.LapsRemaining ? thisCarLapsRemaining : this.LapsRemaining;

                            // Has driver finished?
                            if (this.driverResults[carId].Laps >= this.RaceType.RaceLimitValue)
                            {
                                // Check if first driver to finish
                                this.winningTime = this.winningTime == this.zeroLapTime ? raceDuration : this.winningTime;

                                this.driverResults[carId].Finished = true;
                                this.driverResults[carId].Position = this.finishPosition++;
                                this.driverResults[carId].TotalTime = raceDuration;
                                this.driverResults[carId].TimeOffPace = raceDuration - this.winningTime;
                            }

                            // Have all drivers finished?
                            bool raceFinished = true;
                            foreach (KeyValuePair<int, DriverResult> player in this.driverResults)
                            {
                                raceFinished = player.Value.Finished;
                            }

                            if (raceFinished)
                            {
                                this.FinishRace();
//                                SimpleIoc.Default.GetInstance<RaceHUDViewModel>().RaceFinished();
                            }
                        }
                        else
                        {
                            if (raceDuration >= this.RaceType.RaceLength)
                            {
                                if (this.winningTime == this.zeroLapTime)
                                {
                                    this.winningTime = raceDuration;
                                }
                                this.driverResults[carId].Finished = true;
                                this.driverResults[carId].TotalTime = raceDuration;


                                var playersList = this.DriverResults.Values.ToList();
                                foreach(DriverResult driver in playersList.OrderByDescending(d => d.Laps))
                                {
                                    driver.Position = this.finishPosition++;
                                    driver.Finished = true;
                                    driver.TotalTime = raceDuration;
                                }

                                this.FinishRace();
//                                SimpleIoc.Default.GetInstance<RaceHUDViewModel>().RaceFinished();
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"CarId {carId + 1} : {lapTime}");
                    }
                }
            }
            else
            {
                this.QuitRace();
            }
        }

        /// <summary>
        /// Sets the starting game timer for all driver's previous game counter value.
        /// </summary>
        /// <param name="gameTimer">The starting game timer.</param>
        internal void SetRaceStartGameTimer(UInt32 gameTimer)
        {
            this.StartingGameTimer = gameTimer;
            foreach (KeyValuePair<int, DriverResult> player in this.driverResults)
            {
                player.Value.PreviousGameCounter = gameTimer;
            }
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
