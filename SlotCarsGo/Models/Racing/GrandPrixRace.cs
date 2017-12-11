
namespace SlotCarsGo.Models.Racing
{
    using System;

    class GrandPrixRace : RaceTypeBase
    {

        public GrandPrixRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.Name = "Grand Prix";
            this.Rules = "Multiple players race against each other to complete the set number of laps in the fastest time or the most laps in the set number of minutes.";
            this.Symbol = Windows.UI.Xaml.Controls.Symbol.Flag;
        }
    }
}
