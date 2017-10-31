using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMDigitalSlotCarsSystem
{
    class FreePlayRace : RaceTypeBase
    {

        public FreePlayRace(int numOfLaps, TimeSpan durationMins, bool lapsNotDuration)
        {
            this.RaceTypeName = "Free Play";
        }
    }
}
