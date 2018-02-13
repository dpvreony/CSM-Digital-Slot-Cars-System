using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo.Models.Comms
{
    /// <summary>
    /// Represents a drive packet received from a handset or transmitted to the powerbase.
    /// Each byte carries the status of the corresponding handset, which also indicates 
    /// driving power, braking and lane change operation.
    /// ==================================================================================
    /// bit7 = "1" applies brake
    /// bit6 = "1" requests lane change
    /// bit5-bit0 = required power level (0-63)
    /// </summary>
    class DrivePacket
    {
        public static BitVector32.Section powerSection = BitVector32.CreateSection(63);
        byte packet;
        BitVector32 bits;

        internal bool Brake { get; set; }
        internal bool LaneChange { get; set; }
        internal byte Power { get; set; }

        /// <summary>
        /// Updates the Drive Packets properties using last received byte from handset.
        /// </summary>
        /// <param name="b">The byte data corresponding to a handset.</param>
        public DrivePacket(byte b)
        {
            //TODO: Ones complement (~b)?
            this.packet = b;
            this.bits = new BitVector32(b);
            this.LaneChange = bits[6];
            this.Brake = bits[7];
            this.Power = (byte)bits[powerSection];
        }
    }
}
