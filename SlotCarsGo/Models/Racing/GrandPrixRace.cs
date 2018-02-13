
namespace SlotCarsGo.Models.Racing
{
    using System;
    using static SlotCarsGo.Helpers.Enums;

    class GrandPrixRace : RaceTypeBase
    {
        public GrandPrixRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.raceTypeEnum = RaceTypesEnum.GP;
            this.name = "Grand Prix";
            this.rules = "Multiple players race against each other to complete the set number of laps in the fastest time or the most laps in the set number of minutes.";
            this.symbol = Windows.UI.Xaml.Controls.Symbol.Flag;
        }
    }
}
