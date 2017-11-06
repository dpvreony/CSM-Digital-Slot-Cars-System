using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem
{
    public class Enums
    {
        public enum RaceTypes { FreePlay, Qualifying, GP, Timetrial };
        public enum PacketType { Outgoing = 9, Incoming = 15 };
        public enum MessageBytes : byte { ZeroByte = 0x00, SuccessByte = 0xFF, NotRecognisedByte = 0x7F };

    }
}
