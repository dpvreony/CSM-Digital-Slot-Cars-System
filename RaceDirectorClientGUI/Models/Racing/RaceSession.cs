using CSMDigitalSlotCarsSystem.Models.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static CSMDigitalSlotCarsSystem.Enums;

namespace CSMDigitalSlotCarsSystem
{
    class RaceSession
    {
        const float GameTimerConstant = 6.4f;

        private ActionBlock<byte[]> processIncomingActionBlock;
        private string sessionName;
        private uint trackID; // TODO: Retrieve from XML on startup and store above Powerbase? RaceManager class?
        private RaceTypeBase raceType;
        private Powerbase powerbase;
        private List<Player> players;
        private DateTime startTime;
        private DateTime endTime;
        internal bool Started { get; set; }
        internal bool Finished { get; set; }
        private UInt32 StartingGameTimer { get; set; }
        // TODO: fastest lap of session - what format, to include driver?
        private List<TimeSpan>[] driversLapTimes;
        private TimeSpan[] driversPreviousLapTime;
        private TimeSpan[] driversFastestLapTimes;
        private List<UInt32>[] driversLapGameCounters;
        private UInt32[] driversPreviousGameCounter;
        private bool[] driversFinished;
        private bool fuelEnabled;
        private float[] playerFuel; // TODO calculate throttle value based on fuel level (needs to factor fuel effect)

        // New Race session GUI creates a configured race type and passses to session?

        /// <summary>
        /// Constructor for a race session.
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="raceType"></param>
        /// <param name="players"></param>
        public RaceSession(uint trackID, RaceTypeBase raceType, List<Player> players)
        {
            this.ProcessIncomingActionBlock = new ActionBlock<byte[]>(input => ProcessIncomingPacketFromPowerbase(input));
            this.TrackID = trackID;
            this.RaceType = raceType;
            this.Players = players;
            this.StartTime = DateTime.Now;
            this.Started = false;
            this.Finished = false;
            this.DriversLapTimes = new List<TimeSpan>[] { new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>(), new List<TimeSpan>() };
            this.DriversPreviousLapTime = new TimeSpan[] { new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan() };
            this.DriversFastestLapTimes = new TimeSpan[] { new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan(), new TimeSpan() };
            this.DriversPreviousGameCounter = new UInt32[] { 0, 0, 0, 0, 0, 0 };
            this.DriversLapGameCounters = new List<UInt32>[] { new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>(), new List<UInt32>() };
            this.DriversFinished = new bool[] { false, false, false, false, false, false };
        }

        public ActionBlock<byte[]> ProcessIncomingActionBlock { get => this.processIncomingActionBlock; set => this.processIncomingActionBlock = value; }

        public uint TrackID { get => this.trackID; set => this.trackID = value; }

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



        /// <summary>
        /// Starts a race session, which restarts the Powerbase comms and initiates data recording.
        /// </summary>
        /// <param name="powerbase">The Powerbase instance handling the comms.</param>
        public void RaceStart(Powerbase powerbase)
        {
            if (powerbase != null)
            {
                this.powerbase = powerbase;
                this.StartTime = DateTime.Now;
                this.powerbase.Run(this);
            }
            else
            {
                throw new Exception("Powerbase is not initialised.");
            }
        }

        /// <summary>
        /// Finishes the race including stopping the comms to the Powerbase.
        /// </summary>
        public void RaceFinish()
        {
            this.powerbase.CancelPowerbaseDataFlow();
        }

        /// <summary>
        /// Receieves valid incoming packets from Powerbase for adding to a 
        /// BufferBlock for processing laptimes while returning an outgoing message
        /// with throttle levels adjusted for fuel.
        /// </summary>
        /// <param name="incoming">The incoming packet.</param>
        /// <returns>The outgoing packet.</returns>
        public async Task<byte[]> ReceiveIncomingPacketFromPowerbase(byte[] incoming)
        {
            await this.AddToProcessInputActionBlock(incoming);

            byte[] outgoingPacket = {
                (byte)MessageBytes.SuccessByte,
                incoming[1], // TODO: Calculate fuel effect - beware of ones complement
                incoming[2], // TODO: Throttle finished drivers?
                incoming[3], // TODO: Safety car throttling - timer since last throttle > 0? 2 secs
                incoming[4],
                incoming[5],
                incoming[6],
                incoming[7] = (byte)CalculateLEDStatusLights(players.Count), // Green & Car 1
                // TODO: Calculate every time?
                (byte)MessageBytes.ZeroByte
            };

            return outgoingPacket;
        }

        /// <summary>
        /// Processes the incoming packet from the Powerbase to check for initial 
        /// starting game counter and for any cars that have crossed the finish line.
        /// </summary>
        /// <param name="incomingPacket">The incoming packet.</param>
        private void ProcessIncomingPacketFromPowerbase(byte[] incomingPacket)
        {
            try
            {
                byte carIdOnFinishLine = (byte)(incomingPacket[8] - 248); // Least significant 3 bits
                if (carIdOnFinishLine == 0)
                {
                    if (!this.Started)
                    {
                        UInt32 gameTimer = this.ConvertBytesToGameTimerValue(
                            new byte[] {
                                incomingPacket[12],
                                incomingPacket[11],
                                incomingPacket[10],
                                incomingPacket[9]
                            });
                        this.StartingGameTimer = gameTimer;
                        this.DriversPreviousGameCounter[0] = gameTimer;
                        this.DriversPreviousGameCounter[1] = gameTimer;
                        this.DriversPreviousGameCounter[2] = gameTimer;
                        this.DriversPreviousGameCounter[3] = gameTimer;
                        this.DriversPreviousGameCounter[4] = gameTimer;
                        this.DriversPreviousGameCounter[5] = gameTimer;
                        this.Started = true;
                    }
                }
                else if (carIdOnFinishLine <= 6)
                {
                    carIdOnFinishLine -= 1; // Adjust for zero index arrays

                    UInt32 gameTimer = this.ConvertBytesToGameTimerValue(
                        new byte[] {
                            incomingPacket[12],
                            incomingPacket[11],
                            incomingPacket[10],
                            incomingPacket[9]
                        });

                    this.ProcessFinishLineData(carIdOnFinishLine, gameTimer);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Processes the finsh line data packet to record laptimes and monitor race progress.
        /// </summary>
        /// <param name="carId">The carId.</param>
        /// <param name="gameTimer">The game timer data.</param>
        private void ProcessFinishLineData(int carId, UInt32 gameTimer)
        {
            // Add to Driver's game counters list
            this.DriversLapGameCounters[carId].Add(gameTimer);

            // Calculate last lap as a counter;
            UInt32 lastLapAsGameTimer = gameTimer - this.DriversPreviousGameCounter[carId];

            // Update driver's previous game counter
            this.DriversPreviousGameCounter[carId] = gameTimer;

            // Calculate last lap counter as a timespan
            TimeSpan lapTime = this.ConvertGameTimerToTimeSpan(lastLapAsGameTimer);

            // Add last lasp time to driver's lap times list
            this.DriversLapTimes[carId].Add(lapTime);

            // Update driver's previous lap time
            this.DriversPreviousLapTime[carId] = lapTime;

            // Check if last lap time is this driver's fastest in this session
            if (lapTime > this.DriversFastestLapTimes[carId])
            {
                this.DriversFastestLapTimes[carId] = lapTime;
            }

            // Check for race end conditions
            if (this.RaceType.LapsNotDuration)
            {
                if (this.DriversLapTimes[carId].Count >= this.RaceType.NumberOfLaps)
                {
                    this.Finished = true;
                    this.DriversFinished[carId] = true;
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

            Console.WriteLine($"CarId {carId + 1} : {lapTime}");
            //TODO: decide when to kill Powerbase comms (EndRace())
        }

        /// <summary>
        /// Converts the 4 game timer bytes to a UInt32 number.
        /// </summary>
        /// <param name="gameTimerBytes">The 10th-13th bytes containing the 32-bit game timer value.</param>
        /// <returns>The 32-bit game timer value.</returns>
        internal UInt32 ConvertBytesToGameTimerValue(byte[] gameTimerBytes)
        {
            return BitConverter.ToUInt32(
                new byte[] { gameTimerBytes[3], gameTimerBytes[2], gameTimerBytes[1], gameTimerBytes[0] },
                0);
        }

        /// <summary>
        /// Converts the game timer 32 bit number from microseconds in to milliseconds 
        /// and returns as a TimeSpan object. 
        /// </summary>
        /// <param name="gameTimer"></param>
        /// <returns>The Game Timer as a TimeSpan</returns>
        internal TimeSpan ConvertGameTimerToTimeSpan(UInt32 gameTimer)
        {
            return new TimeSpan(0, 0, 0, 0, (int)(gameTimer * RaceSession.GameTimerConstant) / 1000);
        }

        /// <summary>
        /// Asynchronously adds incoming bytes to the ProcessIncomingActionBlock
        /// while returning control to the calling method.
        /// </summary>
        /// <param name="incoming"></param>
        private async Task AddToProcessInputActionBlock(byte[] incoming)
        {
            await this.ProcessIncomingActionBlock.SendAsync(incoming);
        }

        /// <summary>
        /// Calculates the LED status byte based on number of active players in sesion. 
        /// </summary>
        /// <param name="numPlayers">The number of active players.</param>
        /// <returns>the LED status byte</returns>
        internal int CalculateLEDStatusLights(int numPlayers)
        {
            int ledStatus = 128; // Default Powerbase Green Light
            switch (numPlayers)
            {
                case 1:
                    ledStatus += 1;
                    break;
                case 2:
                    ledStatus += 2 + 1;
                    break;
                case 3:
                    ledStatus += 4 + 2 + 1;
                    break;
                case 4:
                    ledStatus += 8 + 4 + 2 + 1;
                    break;
                case 5:
                    ledStatus += 16 + 8 + 4 + 2 + 1;
                    break;
                case 6:
                    ledStatus += 32 + 16 + 8 + 4 + 2 + 1;
                    break;
            }
            return ledStatus;
        }
    }
}
