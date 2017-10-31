namespace CSMDigitalSlotCarsSystem.Models.Comms
{
    using System;
    using System.IO;
    using System.IO.Ports;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using static CSMDigitalSlotCarsSystem.Enums;

    /// <summary>
    /// The Powerbase class manages all IO to and from the Powerbase device over a USB 
    /// Serial Port connection, according to the "C7042 Scalextric 6 Car Power Base SNC
    /// Communication Protocol v1" produced by "Sagentia" for "Hornby plc". 
    /// Incoming Packets as byte arrays are processed by an asynchronous ActionBlock.
    /// </summary>
    class Powerbase
    {
        private ActionBlock<byte[]> IncomingPacketActionBlock;
        private OutgoingPacket SuccessOutgoingPacket = new OutgoingPacket(true);
        private OutgoingPacket NotRecognisedOutgoingPacket = new OutgoingPacket(false);
        private SerialPort port;
        private RaceSession raceSession;
        public int msgRcvdCnt = 0;
        public int msgOutCnt = 0;

 
        /// <summary>
        /// Static contructor of the Powerbase class, which initiliases an ActionBlock 
        /// to manage IO from the Powerbase device.
        /// </summary>
        public Powerbase()
        {
            try
            {
                this.Port = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One); //TODO port number may change
                this.Port.WriteBufferSize = 1024;
                this.Port.ReadBufferSize = 1024;
                this.Port.ReceivedBytesThreshold = 15;
                this.Port.Open();
                this.Port.DtrEnable = true;
                this.Port.RtsEnable = true;
                this.Port.BaseStream.WriteTimeout = 2000;
                this.Port.BaseStream.ReadTimeout = 2000;
                this.Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);

                this.SuccessOutgoingPacket.Checksum = this.CalculateCrcChecksum(SuccessOutgoingPacket.Data, PacketType.Outgoing);
                this.NotRecognisedOutgoingPacket.Checksum = this.CalculateCrcChecksum(NotRecognisedOutgoingPacket.Data, PacketType.Outgoing);
            }
            catch(UnauthorizedAccessException e)
            {

            }
            catch (IOException e)
            {

            }
        }

        public SerialPort Port { get => port; set => port = value; }

        public RaceSession RaceSession { get => raceSession; set => raceSession = value; }

        /// <summary>
        /// Initiates comms with Powerbase hardware by sending a successful outgoing packet.
        /// </summary>
        public async void Run(RaceSession session)
        {
            this.RaceSession = session;
            this.IncomingPacketActionBlock = new ActionBlock<byte[]>(input => ProcessIncomingPacket(input));
            await this.Port.BaseStream.WriteAsync(SuccessOutgoingPacket.Data, 0, (int)PacketType.Outgoing);
        }

        /// <summary>
        /// Sends a signal to the Powerbase incoming ActionBlock to stop processing messages.
        /// </summary>
        public void CancelPowerbaseDataFlow()
        {
            IncomingPacketActionBlock.Complete();
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
        /// Event handler for data received events
        /// </summary>
        /// <param name="sender">The serial port sender.</param>
        /// <param name="e">The serial data received event args.</param>
        private async void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                System.Diagnostics.Debug.WriteLine($"Receive Success {++msgRcvdCnt}");
                var serialPort = sender as SerialPort;
                var bytesToRead = serialPort.BytesToRead;
                var data = new byte[bytesToRead];
                serialPort.Read(data, 0, bytesToRead);
                if (data.Length == (int)PacketType.Incoming)
                {
                    // CRC check, send notunderstood if fails
                    if (data[(int)PacketType.Incoming - 1] == this.CalculateCrcChecksum(data, PacketType.Incoming))
                    {
                        await this.IncomingPacketActionBlock.SendAsync(data);
                    }
                    else
                    {
                        this.SendUnrecognisedOutgoingPacket();
                        System.Diagnostics.Debug.WriteLine("Sent Unrecognised");
                        msgRcvdCnt -= 1;
                    }
                }
            }
        }

        /// <summary>
        /// Processes the IncomingPackets after a new packet is added to the Incoming ActionBlock.
        /// </summary>
        private async void ProcessIncomingPacket(byte[] incoming)
        {
            // Send to game manager, which will process SF line and return modified outgoing packets
            var outgoingPacket = this.RaceSession.ReceiveIncomingPacketFromPowerbase(incoming).Result;
            
            // Calculate CRC for new outgoing packet
            outgoingPacket[8] = this.CalculateCrcChecksum(outgoingPacket, PacketType.Outgoing);

            // Send to the Powerbase
            await this.Port.BaseStream.WriteAsync(outgoingPacket, 0, (int)PacketType.Outgoing);
            System.Diagnostics.Debug.WriteLine($"Sent Success {++this.msgOutCnt}");
        }

        /// <summary>
        /// Sends an UnrecognisedPacket to the Powerbase.
        /// </summary>
        private async void SendUnrecognisedOutgoingPacket()
        {
            await this.Port.BaseStream.WriteAsync(NotRecognisedOutgoingPacket.Data, 0, (int)PacketType.Outgoing);
        }
    }
}
