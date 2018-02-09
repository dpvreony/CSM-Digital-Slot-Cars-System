
namespace SlotCarsGo.Helpers
{
    public class Enums
    {
        public enum RaceTypes { FreePlay, Qualifying, GP, Timetrial };
        public enum PacketType { Outgoing = 9, Incoming = 15 };
        public enum MessageBytes : byte
        {
            ZeroByte = 0,
            NotRecognisedByte = 127,
            MaxThrottleTimeout = 10, // was 204 before BitVector sectioning
            FinishedDriverThrottle = 50, // Was 220
            NoThrottle = 0x7F, // 0x7F, // was 127 (should be 63?)
            GameTimerStarted = 128,
            GameTimerStopped = 192,
            CarIdentifierMask= 248,
            SuccessByte = 255
        };

    }
}
