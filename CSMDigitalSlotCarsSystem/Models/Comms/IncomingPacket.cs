using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem.Models.Comms
{
    /// <summary>
    /// The incoming packet (from Powerbase to PC) carries the information of:
    /// - Current status of track and each handset
    /// - Auxiliary port current consumed
    /// - Game Timer information
    /// - Car information updated, includes car ID and passing SF-line time
    /// </summary>
    class IncomingPacket
    {
        private const byte SuccessByte = 0xFF;
        private const byte NotRecognisedByte = 0x7F;

        internal byte[] Data { get; set; }
        internal HandsetTrackStatusPacket HandsetTrackStatus { get; set; }
        internal DrivePacket DrivePacket1 { get; set; }
        internal DrivePacket DrivePacket2 { get; set; }
        internal DrivePacket DrivePacket3 { get; set; }
        internal DrivePacket DrivePacket4 { get; set; }
        internal DrivePacket DrivePacket5 { get; set; }
        internal DrivePacket DrivePacket6 { get; set; }
        internal byte AuxPortCurrent {get; set;}
        internal byte CarIdOnFinishLine { get; set; }
        internal UInt32 TimerCounter { get; set; }
        internal byte Checksum { get; set; }

        public IncomingPacket(byte[] input)
        {
            this.Data = input;
            this.HandsetTrackStatus = new HandsetTrackStatusPacket(input[0]);
            switch (RaceSession.NumPlayers)
            {
                case 6:
                    this.DrivePacket6 = new DrivePacket(input[6]);
                    goto case 5;
                case 5:
                    this.DrivePacket5 = new DrivePacket(input[5]);
                    goto case 4;
                case 4:
                    this.DrivePacket4 = new DrivePacket(input[4]);
                    goto case 3;
                case 3:
                    this.DrivePacket3 = new DrivePacket(input[3]);
                    goto case 2;
                case 2:
                    this.DrivePacket2 = new DrivePacket(input[2]);
                    goto case 1;
                case 1:
                    this.DrivePacket1 = new DrivePacket(input[1]);
                    break;
                default:
                    break;
            }

            this.AuxPortCurrent = input[7];
            this.CarIdOnFinishLine = (byte)(input[8] - 248); // Least significant 3 bits

            if (this.CarIdOnFinishLine == 0)
            {
                // val is GameTimer, else carId
            }
            else if (this.CarIdOnFinishLine < 7)
            {
                this.TimerCounter = BitConverter.ToUInt32(new byte[] { input[12], input[11], input[10], input[9] }, 0);
            }
        }
    }
}
