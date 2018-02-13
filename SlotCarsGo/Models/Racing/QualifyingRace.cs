
namespace SlotCarsGo.Models.Racing
{
    using System;
    using static SlotCarsGo.Helpers.Enums;

    class QualifyingRace : RaceTypeBase
    {
        public QualifyingRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.raceTypeEnum = RaceTypesEnum.Qualifying;
            this.name = "Qualifying";
            this.rules = "Players race one at a time or all together to record the fastest lap in the session. The results can be used to decide the grid order of a Grand Prix race.";
            this.symbol = Windows.UI.Xaml.Controls.Symbol.FourBars;
        }
    }
}
