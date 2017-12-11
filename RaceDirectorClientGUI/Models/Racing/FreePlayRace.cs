
namespace SlotCarsGo_GUI.Models.Racing
{
    using System;

    class FreePlayRace : RaceTypeBase
    {

        public FreePlayRace(int numOfLaps, TimeSpan durationMins, bool lapsNotDuration) 
            : base (numOfLaps, durationMins, lapsNotDuration)
        {
            this.RaceTypeName = "Free Play";
        }
    }
}
