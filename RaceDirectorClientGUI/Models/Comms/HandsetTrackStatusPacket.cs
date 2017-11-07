
namespace RaceDirectorClientGUI.Models.Comms
{
    using System.Collections.Specialized;

    class HandsetTrackStatusPacket
    {
        internal byte Data { get; set; }
        private BitVector32 bits;

        internal bool Track { get; set; } = false;
        internal bool Handset1 { get; set; } = false;
        internal bool Handset2 { get; set; } = false;
        internal bool Handset3 { get; set; } = false;
        internal bool Handset4 { get; set; } = false;
        internal bool Handset5 { get; set; } = false;
        internal bool Handset6 { get; set; } = false;

        public HandsetTrackStatusPacket(byte b)
        {
            this.Data = b;
            this.bits = new BitVector32(b);
            this.Track = bits[0];
            this.Handset1 = bits[1];
            this.Handset2 = bits[2];
            this.Handset3 = bits[3];
            this.Handset4 = bits[4];
            this.Handset5 = bits[5];
            this.Handset6 = bits[6];
        }
    }
}
