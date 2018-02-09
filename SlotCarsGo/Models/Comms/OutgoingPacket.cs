
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Models.Comms
{
    class OutgoingPacket
    {
        private byte[] data = new byte[(int)PacketType.Outgoing];

        internal byte[] Data { get => data; set => data = value; }
        internal byte OperationMode { get => this.data[0]; set => data[0] = value; }
        internal byte LEDStatus { get => this.data[7]; set => this.data[7] = value; }
        internal byte Checksum { get => this.data[8]; set => this.data[8] = value; }




        /// <summary>
        /// Constructs an outgoing packet, either a successful response or a request to resend.
        /// </summary>
        /// <param name="success">Whether a successful response or a request to resend.</param>
        public OutgoingPacket(bool success)
        {
            if (success)
            {
                this.Data = new byte[] {
                    (byte)MessageBytes.SuccessByte,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    0x80,
                    (byte)MessageBytes.ZeroByte
                };


            }
            else
            {
                this.Data = new byte[] {
                    (byte)MessageBytes.NotRecognisedByte,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    (byte)MessageBytes.NoThrottle,
                    0x80,
                    (byte)MessageBytes.ZeroByte
                };
            }
        }
/*
        /// <summary>
        /// Creates a new OutgoingPacket from 9 selected bytes of an IncomingPacket 15 byte array.
        /// </summary>
        /// <param name="output">An IncomingPacket's 15 byte array</param>
        public OutgoingPacket(byte[] output)
        {
            this.Data[0] = SuccessByte;
            for (int i=1; i < 7; i++)
            {
                this.Data[i] = output[i];
            }
            this.Data[7] = 129; // Green & Car 1
//            this.Data[8] = Powerbase.CrcCheck(this.Data, PacketType.Outgoing);


            this.OperationMode = SuccessByte;
            switch(RaceSession.NumPlayers)
            {
                case 6:
                    this.DrivePacket6 = new DrivePacket(output[6]);
                    goto case 5;
                case 5:
                    this.DrivePacket5 = new DrivePacket(output[5]);
                    goto case 4;
                case 4:
                    this.DrivePacket4 = new DrivePacket(output[4]);
                    goto case 3;
                case 3:
                    this.DrivePacket3 = new DrivePacket(output[3]);
                    goto case 2;
                case 2:
                    this.DrivePacket2 = new DrivePacket(output[2]);
                    goto case 1;
                case 1:
                    this.DrivePacket1 = new DrivePacket(output[1]);
                    break;
                default:
                    break;
            }
        }
*/        
    }
}
