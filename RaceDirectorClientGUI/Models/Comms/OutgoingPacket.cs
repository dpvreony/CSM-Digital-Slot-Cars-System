
namespace RaceDirectorClientGUI.Models.Comms
{
    class OutgoingPacket
    {
        private const byte SuccessByte = 0xFF;
        private const byte NotRecognisedByte = 0x7F;
        private const byte ZeroByte = 0x00;
        private byte[] data = new byte[9];

        internal byte[] Data { get => data; set => data = value; }
        internal byte OperationMode { get => this.data[0]; set => data[0] = value; }
        internal DrivePacket DrivePacket1 { get; set; }
        internal DrivePacket DrivePacket2 { get; set; }
        internal DrivePacket DrivePacket3 { get; set; }
        internal DrivePacket DrivePacket4 { get; set; }
        internal DrivePacket DrivePacket5 { get; set; }
        internal DrivePacket DrivePacket6 { get; set; }
        internal byte LEDStatus { get => this.data[7]; set => this.data[7] = value; }
        internal byte Checksum { get => this.data[8]; set => this.data[8] = value; }

        public OutgoingPacket(bool success)
        {
            if (success)
            {
                this.Data = new byte[] { SuccessByte, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0x27 };
            }
            else
            {
                this.Data = new byte[] { NotRecognisedByte, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x80, 0x98 };
            }
        }

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

/*
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
*/
        }
    }
}
