namespace SlotCarsGo.Models.Comms
{
    using SlotCarsGo.Models.Racing;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Windows.Devices.SerialCommunication;
    using Windows.Foundation;
    using Windows.Storage.Streams;
    using static SlotCarsGo.Helpers.Enums;

    /// <summary>
    /// The Powerbase class manages all IO to and from the Powerbase device over a USB 
    /// Serial Port connection, according to the "C7042 Scalextric 6 Car Power Base SNC
    /// Communication Protocol v1" produced by "Sagentia" for "Hornby plc". 
    /// Incoming Packets as byte arrays are processed by an asynchronous ActionBlock.
    /// </summary>
    class Powerbase
    {
        private SerialDevice serialDevice;
        private DataReader reader;
        private DataWriter writer;
        private byte ledStatusLightByte;
        private int msgOutCnt = 0;
        private bool sendReset = false;
        private RaceSession raceSession;
        private BitVector32.Section powerSection = BitVector32.CreateSection(63);
        private ActionBlock<byte[]> incomingPacketActionBlock;
        private ActionBlock<byte[]> updateRaceSessionActionBlock;
        private OutgoingPacket SuccessOutgoingPacket = new OutgoingPacket(true);
        private OutgoingPacket NotRecognisedOutgoingPacket = new OutgoingPacket(false);
        private OutgoingPacket resetGameTimerPacket = new OutgoingPacket(true);
        private CancellationTokenSource powerbaseListenerCancellationTokenSource;
        private TypedEventHandler<SerialDevice, ErrorReceivedEventArgs> ErrorReceivedEventHandler;


        /// <summary>
        /// Constructs the Powerbase class to manage IO from the Powerbase device.
        /// </summary>
        public Powerbase()
        {
            this.resetGameTimerPacket.LEDStatus = (byte)MessageBytes.GameTimerStopped;
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString()}: Powerbase constructor.");
        }

        public RaceSession RaceSession { get => this.raceSession; }
        public bool IsPowerbaseConnected { get => this.serialDevice != null; }

        /// <summary>
        /// Initialises the Serial Port as a device using Vendor Id and Product Id and configures
        /// it's parameters.
        /// </summary>
        /// <returns>The new Serial Device.</returns>
        private async Task<SerialDevice> InitialiseDevice()
        {
            var deviceSelector = SerialDevice.GetDeviceSelector();
            var deviceList = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(deviceSelector);

            if (deviceList.Count == 0)
            {
                return null;
            }
            else
            {
                try
                {
                    SerialDevice serialDevice = await SerialDevice.FromIdAsync(deviceList[0].Id);
                    serialDevice.BaudRate = 19200;
                    serialDevice.DataBits = 8;
                    serialDevice.Handshake = SerialHandshake.None;
                    serialDevice.Parity = SerialParity.None;
                    serialDevice.ReadTimeout = new TimeSpan(0, 0, 0, 0);
                    serialDevice.StopBits = SerialStopBitCount.One;
                    serialDevice.WriteTimeout = new TimeSpan(0, 0, 0, 0);
                    this.ErrorReceivedEventHandler = new TypedEventHandler<SerialDevice, ErrorReceivedEventArgs>(this.ErrorReceivedEvent);
                    serialDevice.ErrorReceived += this.ErrorReceivedEventHandler;

                    return serialDevice;

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("SerialPort not found, check Manifest." + e.Message);
                }
            }

            return null;
        }

        /// <summary>
        /// Initialises the Serial Device connection to the Powerbase, and returns its success.
        /// </summary>
        public async Task<bool> Connect()
        {
            try
            {
                // Try to initialise the serial device
                this.serialDevice = await this.InitialiseDevice();

                if (this.serialDevice != null)
                {
                    System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString()}: Powerbase connected.");

                    // Instantiate new action blocks for buffering new packets
//                    this.incomingPacketActionBlock = new ActionBlock<byte[]>(async input => await this.ProcessIncomingPacket(input));
//                    this.updateRaceSessionActionBlock = new ActionBlock<byte[]>(input => this.UpdateRaceSessionData(input));

                    this.reader = new DataReader(serialDevice.InputStream);
                    this.reader.InputStreamOptions = InputStreamOptions.Partial;
                    this.writer = new DataWriter(serialDevice.OutputStream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Powerbase: failed to connect - {e.Message}");
            }

            return this.serialDevice != null;
        }

        /// <summary>
        /// Stops listening to messages from the Powerbase.
        /// </summary>
        public void StopListening()
        {
            // Pause to allow reset command to be sent
            Task.Delay(500); 
            this.CancelListening();
        }


        /// <summary>
        /// Refreshes the reference to the race session witha  new instance for a new race.
        /// </summary>
        /// <param name="session"></param>
        public void UpdateRaceSession(RaceSession session)
        {
            this.raceSession = session;
            this.ledStatusLightByte = (byte)this.CalculateLEDStatusLights(false, this.raceSession.NumberOfDrivers);
        }


        /// <summary>
        /// Resets the game timer for a new race session.
        /// </summary>
        public void ResetGameTimer()
        {
            this.sendReset = true;
        }

        /// <summary>
        /// Listens for incoming messages that will be sent to the IncomingActionBlock for processing.
        /// </summary>
        public async void Listen(RaceSession session)
        {
            this.raceSession = session;
            this.updateRaceSessionActionBlock = new ActionBlock<byte[]>(input => this.UpdateRaceSessionData(input));

            this.powerbaseListenerCancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = this.powerbaseListenerCancellationTokenSource.Token;

            CancellationTokenSource readCancellationTokenSource = new CancellationTokenSource();
            readCancellationTokenSource.CancelAfter(1000);

            // Calculate LED lights for number of players
            this.ledStatusLightByte = (byte)this.CalculateLEDStatusLights(true, this.raceSession.NumberOfDrivers);

            // Assign LED light status to standard response type packets
            this.resetGameTimerPacket.LEDStatus = this.ledStatusLightByte;
            this.SuccessOutgoingPacket.LEDStatus = this.ledStatusLightByte;
            this.NotRecognisedOutgoingPacket.LEDStatus = this.ledStatusLightByte;

            // Calculate new CRC for standard response type packets
            this.resetGameTimerPacket.Checksum = this.CalculateCrcChecksum(this.resetGameTimerPacket.Data, PacketType.Outgoing);
            this.SuccessOutgoingPacket.Checksum = this.CalculateCrcChecksum(this.SuccessOutgoingPacket.Data, PacketType.Outgoing);
            this.NotRecognisedOutgoingPacket.Checksum = this.CalculateCrcChecksum(this.NotRecognisedOutgoingPacket.Data, PacketType.Outgoing);

            // Setup first outgoing message packet
            byte[] nextOutgoingPacket = new byte[(int)PacketType.Outgoing];
            nextOutgoingPacket = this.SuccessOutgoingPacket.Data;
            int byteIndex = 0; // For listening
            bool msgReceived = true;

            using (this.serialDevice = await this.InitialiseDevice())
            {
                // Race session listening loop
                while (!token.IsCancellationRequested)
                {
                    if (this.serialDevice != null)
                    {
                        // Connect read/writers to streams
                        this.reader = new DataReader(serialDevice.InputStream);
                        this.reader.InputStreamOptions = InputStreamOptions.None;
                        this.writer = new DataWriter(serialDevice.OutputStream);

                        if (msgReceived)
                        {
                            if (this.RaceSession.Finished)
                            {
                                nextOutgoingPacket = resetGameTimerPacket.Data;
                            }

                            // Send to the Powerbase
                            this.writer.WriteBytes(nextOutgoingPacket);
                            await this.writer.StoreAsync();

                            System.Diagnostics.Debug.WriteLine($"Sent {++this.msgOutCnt}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"message not received, skipping send...");
                        }

                        // Listen to device
                        msgReceived = false;
                        byte[] incomingPacket = new byte[(int)PacketType.Incoming];
                        await reader.LoadAsync((int)PacketType.Incoming);

                        while (reader.UnconsumedBufferLength > 0)
                        {
                            incomingPacket[byteIndex++] = reader.ReadByte();
                        }

                        if (byteIndex > 0)
                        {
                            msgReceived = true;
                            byteIndex = 0;
                        }

                        // CRC check, send notunderstood if fails
                        if (incomingPacket[(int)PacketType.Incoming - 1] == this.CalculateCrcChecksum(incomingPacket, PacketType.Incoming))
                        {
                            System.Diagnostics.Debug.WriteLine($"Packet received.");

                            // Send packet to session
                            await this.updateRaceSessionActionBlock.SendAsync(incomingPacket);

                            // Construct the outgoing packet
                            if (this.RaceSession.Started)
                            {
                                nextOutgoingPacket = new byte[]
                                {
                                    (byte)MessageBytes.SuccessByte,
                                    (byte)MessageBytes.NoThrottle,
                                    (byte)MessageBytes.NoThrottle,
                                    (byte)MessageBytes.NoThrottle,
                                    (byte)MessageBytes.NoThrottle,
                                    (byte)MessageBytes.NoThrottle,
                                    (byte)MessageBytes.NoThrottle,
                                    this.ledStatusLightByte,
                                    (byte)MessageBytes.ZeroByte
                                };

                                foreach (KeyValuePair<int, DriverResult> driver in this.RaceSession.DriverResults)
                                {
                                    BitVector32 bits = new BitVector32(incomingPacket[driver.Key]);
                                    byte power = (byte)bits[this.powerSection]; // No power = 63
                                    bits[this.powerSection] = power > (byte)MessageBytes.MaxThrottleTimeout ? power : (byte)MessageBytes.MaxThrottleTimeout;
                                    nextOutgoingPacket[driver.Key] = (byte)bits.Data;
                                    // TODO: Fuel management                                       outgoingPacket[i] = this.CalculateThrottle(incomingPacket[i], this.RaceSession.PlayerResults[i-1].Finished, this.RaceSession.PlayerResults[i-1].Fuel);
                                }

                                // Calculate CRC for new outgoing packet
                                nextOutgoingPacket[8] = this.CalculateCrcChecksum(nextOutgoingPacket, PacketType.Outgoing);
                            }
                            else
                            {
                                // Session not started yet
                                nextOutgoingPacket = resetGameTimerPacket.Data;
                                System.Diagnostics.Debug.WriteLine("Sent Reset.");
                            }
                        }
                        else
                        {
                            // CRC failed, request re-send
                            nextOutgoingPacket = NotRecognisedOutgoingPacket.Data;
                            System.Diagnostics.Debug.WriteLine($"Sent Unrecognised #{msgOutCnt}");
                        }

                        this.reader.DetachStream();
                        this.writer.DetachStream();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to connect to the serial device.");
                    }
                }

                this.serialDevice.Dispose();
                this.serialDevice = null;
            }
        }

        /// <summary>
        /// Processes the IncomingPackets after a new packet is added to the Incoming ActionBlock.
        /// </summary>
        /// <param name="writer">The DataWriter object for the active serial port stream.</param>
        /// <param name="incoming">The incoming packet of bytes.</param>
        private async Task ProcessIncomingPacket(byte[] incomingPacket)
        {
            if (this.RaceSession != null)
            {
                // Process SF line bytes then update Race Session data
                await this.updateRaceSessionActionBlock.SendAsync(incomingPacket);

                byte[] outgoingPacket;
                if (this.RaceSession.Started)
                {
                    // Construct the outgoing packet
                    outgoingPacket = this.BuildOutgoingPacket(incomingPacket);
                }
                else
                {
                    outgoingPacket = resetGameTimerPacket.Data;
                    System.Diagnostics.Debug.WriteLine("Sent Reset.");
                }
 
                // Send to Powerbase
                await this.SendPacketToPowerbase(outgoingPacket);
            }
        }

        /// <summary>
        /// Updates Race Session data for any cars that have crossed the finish line.
        /// </summary>
        /// <param name="incomingPacket">The incoming packet.</param>
        private void UpdateRaceSessionData(byte[] incomingPacket)
        {
            try
            {
                byte carIdOnFinishLine = (byte)(incomingPacket[8] - MessageBytes.CarIdentifierMask); // Least significant 3 bits
                if (carIdOnFinishLine == 0)
                {
                    if (!this.RaceSession.Started)
                    {
                        UInt32 gameTimer = this.ConvertBytesToGameTimerValue(
                            new byte[] {
                                incomingPacket[12],
                                incomingPacket[11],
                                incomingPacket[10],
                                incomingPacket[9]
                            });

                        this.RaceSession.SetRaceStartGameTimer(gameTimer);
                    }
                }
                else if (carIdOnFinishLine <= 6)
                {
//                    carIdOnFinishLine -= 1; // Adjust for zero index arrays

                    UInt32 gameTimer = this.ConvertBytesToGameTimerValue(
                        new byte[] {
                            incomingPacket[12],
                            incomingPacket[11],
                            incomingPacket[10],
                            incomingPacket[9]
                        });

                    this.RaceSession.ProcessFinishLineData(carIdOnFinishLine , gameTimer);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Construct the outgoing packet based on the incoming packet, adjusted
        /// for fuel and safe limits.
        /// </summary>
        /// <param name="incomingPacket">The incoming packet.</param>
        /// <returns>The adjusted outgoing packet.</returns>
        private byte[] BuildOutgoingPacket(byte[] incomingPacket)
        {

        byte[] outgoingPacket = new byte[]
            {
                (byte)MessageBytes.SuccessByte,
                (byte)MessageBytes.NoThrottle,
                (byte)MessageBytes.NoThrottle,
                (byte)MessageBytes.NoThrottle,
                (byte)MessageBytes.NoThrottle,
                (byte)MessageBytes.NoThrottle,
                (byte)MessageBytes.NoThrottle,
                this.ledStatusLightByte,
                (byte)MessageBytes.ZeroByte
            };

            foreach (KeyValuePair<int, DriverResult> driver in this.RaceSession.DriverResults)
            {
                BitVector32 bits = new BitVector32(incomingPacket[driver.Key]);
                byte power = (byte)bits[this.powerSection]; // No power = 63
//                bits[this.powerSection] = power > (byte)MessageBytes.MaxThrottleTimeout ? power : (byte)MessageBytes.MaxThrottleTimeout;
                outgoingPacket[driver.Key] = (byte)bits.Data;
                // TODO: Fuel management                                       outgoingPacket[i] = this.CalculateThrottle(incomingPacket[i], this.RaceSession.PlayerResults[i-1].Finished, this.RaceSession.PlayerResults[i-1].Fuel);
            }


            // Calculate throttle for each active driver
            for (var i = 1; i <= this.RaceSession.NumberOfDrivers; i++)
            {
                if (this.RaceSession.Started)
                {
                    BitVector32 bits = new BitVector32(incomingPacket[i]);
                    byte power = (byte)bits[this.powerSection]; // No power = 63
                    bits[this.powerSection] = power > (byte)MessageBytes.MaxThrottleTimeout ? power : (byte)MessageBytes.MaxThrottleTimeout;
                    outgoingPacket[i] = (byte)bits.Data;

                }
            }

            return outgoingPacket;
        }

        /// <summary>
        /// Returns the power value from first 5 bytes of a drive packet.
        /// </summary>
        /// <param name="drivePacket">the drive packet to mask.</param>
        /// <returns>The power value from first 5 bytes of a drive packet.</returns>
        internal byte BitMaskPowerByteFromDrivePacket(byte drivePacket)
        {
            BitVector32 bits = new BitVector32(drivePacket);
            return (byte)bits[this.powerSection];
        }

        /// <summary>
        /// Calculates the outgoing byte for drivers throttle byte.
        /// </summary>
        /// <param name="incomingByte">The incoming driver throttle byte.</param>
        /// <param name="finished">Whether the driver has finished.</param>
        /// <param name="fuelLevel">The driver's fuel level.</param>
        /// <returns>The driver throttle byte to send to the Powerbase.</returns>
        internal byte CalculateThrottle(byte incomingByte, bool finished, float fuelLevel)
        {
            BitVector32 bitVector = new BitVector32(incomingByte);
            BitVector32.Section throttleSection = BitVector32.CreateSection(63);
            int throttle = bitVector[throttleSection];
            byte outgoingByte = incomingByte;

            if (finished)
            {
                throttle = (int)MessageBytes.FinishedDriverThrottle;
            }
            else if (throttle < (int)MessageBytes.NoThrottle)
            {
                if (this.RaceSession.FuelEnabled)
                {
                    // Ones compliment so less is more
                    bitVector[throttleSection] = throttle;
                }

                if (throttle < (byte)MessageBytes.MaxThrottleTimeout)
                {
                    bitVector[throttleSection] = (int)MessageBytes.MaxThrottleTimeout;
                }
            }

            return (byte)bitVector.Data;
        }

        /// <summary>
        /// Calculates the LED status byte based on number of active players in sesion. 
        /// </summary>
        /// <param name="numPlayers">The number of active players.</param>
        /// <param name="started">Whether the session has started.</param>
        /// <returns>the LED status byte</returns>
        internal int CalculateLEDStatusLights(bool started, int numPlayers)
        {
            int ledStatus = started ? (byte)MessageBytes.GameTimerStarted : (byte)MessageBytes.GameTimerStopped; // Default Powerbase Green Light

            foreach (KeyValuePair<int, DriverResult> driver in this.RaceSession.DriverResults)
            {
                switch (driver.Value.Driver.ControllerId)
                {
                    case 1:
                        ledStatus += 1;
                        break;
                    case 2:
                        ledStatus += 2;
                        break;
                    case 3:
                        ledStatus += 4;
                        break;
                    case 4:
                        ledStatus += 8;
                        break;
                    case 5:
                        ledStatus += 16;
                        break;
                    case 6:
                        ledStatus += 32;
                        break;
                }
            }

            return ledStatus;
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
        /// Sends an outgoing packet of 9 bytes to the Powerbase.
        /// </summary>
        /// <param name="writer">The DataWriter object for the active serial port stream.</param>
        /// <param name="outgoingPacket">The outgoing packet of bytes.</param>
        /// <returns></returns>
        private async Task SendPacketToPowerbase(byte[] outgoingPacket)
        {
            try
            {
                // Calculate CRC for new outgoing packet
                outgoingPacket[8] = this.CalculateCrcChecksum(outgoingPacket, PacketType.Outgoing);

                // Send to the Powerbase
                this.writer.WriteBytes(outgoingPacket);
                await this.writer.StoreAsync();
                System.Diagnostics.Debug.WriteLine($"Sent Success {++this.msgOutCnt}");
            }
            catch (COMException e)
            {

                System.Diagnostics.Debug.WriteLine($"Powerbase Exception thrown while writing to data writer: {e.Message}");
            }
        }

        /// <summary>
        /// Cancels the token for the Powerbase Listen loop.
        /// </summary>
        private void CancelListening()
        {
            if (powerbaseListenerCancellationTokenSource != null)
            {
                this.powerbaseListenerCancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Performs a CRC8 calculation on the n-1 bytes in an incoming or outgoing packet.
        /// </summary>
        /// <param name="from6CPB_Msg">The packet to generate a checksum for.</param>
        /// <param name="packetType">The type of packet (Incoming or Outgoing).</param>
        /// <returns>The checksum byte.</returns>
        internal byte CalculateCrcChecksum(byte[] from6CPB_Msg, PacketType packetType)
        {
            byte crc8Rx = 0;
            byte nxtCrc8Rx = 0;
            var CRC8_LOOK_UP_TABLE = new byte[]
            {
                0x00,0x07,0x0e,0x09,0x1c,0x1b,0x12,0x15,0x38,0x3f,0x36,0x31,0x24,0x23,0x2a,0x2d,
                0x70,0x77,0x7E,0x79,0x6C,0x6B,0x62,0x65,0x48,0x4F,0x46,0x41,0x54,0x53,0x5A,0x5D,
                0xE0,0xE7,0xEE,0xE9,0xFC,0xFB,0xF2,0xF5,0xD8,0xDF,0xD6,0xD1,0xC4,0xC3,0xCA,0xCD,
                0x90,0x97,0x9E,0x99,0x8C,0x8B,0x82,0x85,0xA8,0xAF,0xA6,0xA1,0xB4,0xB3,0xBA,0xBD,
                0xC7,0xC0,0xC9,0xCE,0xDB,0xDC,0xD5,0xD2,0xFF,0xF8,0xF1,0xF6,0xE3,0xE4,0xED,0xEA,
                0xB7,0xB0,0xB9,0xBE,0xAB,0xAC,0xA5,0xA2,0x8F,0x88,0x81,0x86,0x93,0x94,0x9D,0x9A,
                0x27,0x20,0x29,0x2E,0x3B,0x3C,0x35,0x32,0x1F,0x18,0x11,0x16,0x03,0x04,0x0D,0x0A,
                0x57,0x50,0x59,0x5E,0x4B,0x4C,0x45,0x42,0x6F,0x68,0x61,0x66,0x73,0x74,0x7D,0x7A,
                0x89,0x8E,0x87,0x80,0x95,0x92,0x9B,0x9C,0xB1,0xB6,0xBF,0xB8,0xAD,0xAA,0xA3,0xA4,
                0xF9,0xFE,0xF7,0xF0,0xE5,0xE2,0xEB,0xEC,0xC1,0xC6,0xCF,0xC8,0xDD,0xDA,0xD3,0xD4,
                0x69,0x6E,0x67,0x60,0x75,0x72,0x7B,0x7C,0x51,0x56,0x5F,0x58,0x4D,0x4A,0x43,0x44,
                0x19,0x1E,0x17,0x10,0x05,0x02,0x0B,0x0C,0x21,0x26,0x2F,0x28,0x3D,0x3A,0x33,0x34,
                0x4E,0x49,0x40,0x47,0x52,0x55,0x5C,0x5B,0x76,0x71,0x78,0x7F,0x6A,0x6D,0x64,0x63,
                0x3E,0x39,0x30,0x37,0x22,0x25,0x2C,0x28,0x06,0x01,0x08,0x0F,0x1A,0x1D,0x14,0x13,
                0xAE,0xA9,0xA0,0xA7,0xB2,0xB5,0xBC,0xBB,0x96,0x91,0x98,0x9F,0x8A,0x8D,0x84,0x83,
                0xDE,0xD9,0xD0,0xD7,0xC2,0xC5,0xCC,0xCB,0xE6,0xE1,0xE8,0xEF,0xFA,0xFD,0xF4,0xF3
            };

            var numLoops = packetType == PacketType.Incoming ? (int)PacketType.Incoming : (int)PacketType.Outgoing;
            numLoops -= 1; // Ignore checksum byte
            var i = 0;

            // Routine for the CRC
            crc8Rx = CRC8_LOOK_UP_TABLE[from6CPB_Msg[0]]; //first byte

            for (i = 1; i < numLoops; i++)
            {
                nxtCrc8Rx = from6CPB_Msg[i];
                crc8Rx = CRC8_LOOK_UP_TABLE[crc8Rx ^ nxtCrc8Rx];
            }

            return crc8Rx;
        }

        /// <summary>
        /// Event handler for errors on Serial Device connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private async void ErrorReceivedEvent(SerialDevice sender, ErrorReceivedEventArgs eventArgs)
        {
            // Reset and re-initialise
            this.CancelListening();

            System.Diagnostics.Debug.WriteLine("Error Received Event Received", eventArgs.Error.ToString());

            while (sender != null)
            {
                sender.Dispose();
                System.Diagnostics.Debug.WriteLine("Waiting for Serial Device to dispose.");
            }

            this.serialDevice = await this.InitialiseDevice();
            if (this.serialDevice != null)
            {
                this.Listen(this.RaceSession);
            }
        }
    }
}
