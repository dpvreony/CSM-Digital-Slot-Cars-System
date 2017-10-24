using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem
{
    class RaceSession
    {
        public static int NumPlayers;

        uint TrackID; // TODO: Retrieve from XML on startup and store above Powerbase? RaceManager class?
        Enums.RaceTypes raceType;
        Player[] players;
        DateTime startTime;
        DateTime endTime;
        Int32 TimerCounter { get; set; } // TODO: passed in IncomingPacket but needs recording somewhere. actual time = 6.4 u (millionth) sec x counter val

        public RaceSession(uint trackId, Enums.RaceTypes raceType, Player[] players)
        {
            NumPlayers = players.Count();
            // etc - add more args to constructor
        }
    }
}
