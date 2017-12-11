
namespace SlotCarsGo.Models.Racing
{
    using System;

    class TimeTrialRace : RaceTypeBase
    {

        public TimeTrialRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled) 
            : base (raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.Name = "Time Trial";
            this.Rules = "Players race one at a time in a series to complete the set number of laps in the shortest time.";
            this.Symbol = Windows.UI.Xaml.Controls.Symbol.RepeatAll;
        }
    }
}
