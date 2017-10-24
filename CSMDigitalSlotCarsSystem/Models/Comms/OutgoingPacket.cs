using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem.Models.Comms
{
    class OutgoingPacket
    {
        private const byte SuccessByte = 0xFF;
        private const byte NotRecognisedByte = 0x7F;
        private const byte ZeroByte = 0x00;
        private byte[] data;

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

        /// <summary>
        /// Dummy packet for testing, including checksum value
        /// </summary>
        public OutgoingPacket TestOutgoingPacket()
        {
            return new OutgoingPacket(new byte[] 
            {
                SuccessByte, 
            });
        }

        public OutgoingPacket(bool success)
        {
            if (success)
            {
                this.Data = new byte[]
                {
                    SuccessByte,
                    SuccessByte, SuccessByte, SuccessByte, SuccessByte, SuccessByte,SuccessByte,
                    191,
                    ZeroByte
                };
            }
            else
            {
                this.Data = new byte[]
                {
                    NotRecognisedByte,
                    SuccessByte, SuccessByte, SuccessByte, SuccessByte, SuccessByte,SuccessByte,
                    129,
                    ZeroByte
                };
            }
        }

        public OutgoingPacket(byte[] output)
        {
            this.Data = output;
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
    }
}
