using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDirectorClientGUI.Models.Comms
{
    class SerialDataPacketReadyEventArgs : EventArgs
    {
        private string eventInfo;
        private byte[] packet;

        /// <summary>
        /// Event args for SerialDataPacketReadyEvent.
        /// </summary>
        /// <param name="text">The event info text.</param>
        /// <param name="packet">The incoming packet passed as the event arg.</param>
        public SerialDataPacketReadyEventArgs(string text, byte[] packet)
        {
            this.eventInfo = text;
            this.packet = packet;
        }

        public string GetInfo()
        {
            return this.eventInfo;
        }

        public byte[] GetPacket()
        {
            return this.packet;
        }
    }
}
