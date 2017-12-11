using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo_GUI.Models.Racing
{
    abstract class RaceTypeBase
    {
        internal string RaceTypeName { get; set; }
        internal int NumberOfLaps { get; set; }
        internal TimeSpan RaceLength { get; set; }
        internal bool LapsNotDuration { get; set; }
        internal Int32 CrashPenalty { get; set; }

        /// <summary>
        /// Base constructor for race type base class.
        /// </summary>
        public RaceTypeBase(int numOfLaps, TimeSpan durationMins, bool lapsNotDuration)
        {
            this.NumberOfLaps = numOfLaps;
            this.RaceLength = durationMins;
            this.LapsNotDuration = lapsNotDuration;
        }

    }
}
