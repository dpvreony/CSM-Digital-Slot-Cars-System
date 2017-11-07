
namespace RaceDirectorClientGUI.Models.Comms
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The incoming packet (from Powerbase to PC) carries the information of:
    /// - Current status of track and each handset
    /// - Auxiliary port current consumed
    /// - Game Timer information
    /// - Car information updated, includes car ID and passing SF-line time
    /// =================================================================================
    /// Note the following amendments to the C7042 SNC protocol
    /// Firmware version 0.82 (and all subsequent versions) 
    /// ---------------------------------------------------
    /// introduced an additional byte to the incoming packet identifying if a button had 
    /// been pressed on the powerbase.This additional byte extends the incoming packet 
    /// to 15 bytes, with the button status byte at byte 14 (after the timer and before 
    /// the CRC check). The button status bits are set as follows:
    /// bit 7 = 1 => reserved for future uses; and b6 = 1 => reserved for future uses.
    /// bit 5 = 1 => DOWN button released; 0 = DOWN button pressed;
    /// bit 4 = 1 => LEFT button released; 0 = LEFT button pressed;
    /// bit 3 = 1 => ENTER button released; 0 = ENTER button pressed;
    /// bit 2 = 1 => UP button released; 0 = UP button pressed;
    /// bit 1 = 1 => RIGHT button released; 0 = RIGHT button pressed;
    /// bit 0 = 1 => START button released; 0 = START button pressed;
    /// 
    /// Firmware version 1.05 (and all subsequent versions) 
    /// ---------------------------------------------------
    /// introduced the support the transmission of AUX data, which can be used by the 
    /// InCar-Pro decoder.This AUX data takes the form of a single byte for each car, 
    /// and so the overall packet length remains the same as normal.
    /// In order for the powerbase to recognise that the incoming data is AUX data, the 
    /// operation (first) byte is adjusted as follows:
    /// The most significant bit (bit 7) remains an indication of a read error: set(1) 
    /// for no read error and unset(0) to request a resend.
    /// The next bit (bit 6) indicates the type of packet being sent: set(1) for a DRIVE 
    /// packet and unset(0) for an AUX packet.
    /// Each car byte in the AUX data takes the following format:
    ///         Size    Data
    ///         ---------------------
    /// MS Bit  1 bit   Lights on/off
    ///         1 bit   SP on/off
    ///         1 bit   RC1 on/off
    ///         5 bits  reserved, must be $1F
    /// The C7042 powerbase will retain the last transmitted AUX data and send it out on 
    /// the track every 30 packets.As a result, there is no need to transmit AUX data over 
    /// SNC unless it requires changing.The C7042 powerbase will also respond to an AUX 
    /// packet transmission in the same way as it would to a normal DRIVE packet.
    /// Additional information regarding the InCar-Pro firmware is available at ElectricImages.
    ///
    /// Firmware version 1.07 (and all subsequent versions)
    /// ---------------------------------------------------
    /// introduced the ability to ID cars and control powerbase settings.This is achieved in the following ways:
    /// To ID a car you send a standard drive packet (operation byte, bit 6 set) and then make
    /// use of the LED status byte in the following way:
    ///     Bit 7=0, Bit 6=0: Bits 5-0 indicate the status of the powerbase LEDs.No other effect. (unchanged)
    ///     Bit 7=0, bit 6=1: Bits 5-0 indicate the car to ID(only one bit should be set). ID Car(changed)
    ///     Bit 7=1, bit 6=0: Bits 5-0 indicate the status of the powerbase LEDs.Start Game timer. (unchanged)
    ///     Bit 7=1, bit 6=1: Bits 5-0 indicate the status of the powerbase LEDs.Stop Game timer. (unchanged)
    /// To set powerbase settings you send a standard AUX packet(operation byte, bit 6 unset) 
    /// and then make use of the LED status byte in the following way:
    ///     Bit 7: 0=Digital, 1=Analog
    ///     Bit 6: 0=AUX track data disabled 1=AUX track data enabled
    ///     Bit 5: retained for future use
    ///     Bit 4: retained for future use
    ///     Bit 3: retained for future use
    ///     Bit 2: retained for future use
    ///     Bit 1: retained for future use
    ///     Bit 0: retained for future use
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
        internal byte AuxPortCurrent {get => this.Data[7]; set => this.Data[7] = value; }
        internal byte CarIdOnFinishLine { get; set; }
        internal UInt32 TimerCounter { get; set; }
        internal byte ButtonStatus { get => this.Data[13]; set => this.Data[13] = value; }
        internal byte Checksum { get => this.Data[14]; set => this.Data[14] = value; }

        public IncomingPacket(byte[] input)
        {
            this.Data = input;
/*
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
            this.ButtonStatus = input[13];
*/
        }
    }
}
