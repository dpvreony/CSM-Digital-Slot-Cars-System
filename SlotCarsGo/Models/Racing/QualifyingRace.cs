
namespace SlotCarsGo.Models.Racing
{
    using System;

    class QualifyingRace : RaceTypeBase
    {

        public QualifyingRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.Name = "Qualifying";
            this.Rules = "Players race one at a time or all together to record the fastest lap in the session. The results can be used to decide the grid order of a Grand Prix race.";
            this.Symbol = Windows.UI.Xaml.Controls.Symbol.FourBars;
        }
    }
}
