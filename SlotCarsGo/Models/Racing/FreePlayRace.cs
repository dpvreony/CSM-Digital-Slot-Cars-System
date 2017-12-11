
namespace SlotCarsGo.Models.Racing
{
    using System;

    class FreePlayRace : RaceTypeBase
    {

        public FreePlayRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.Name = "Free Play";
            this.Rules = "Players drive for the fun of it - no limit and no rules!";
            this.Symbol = Windows.UI.Xaml.Controls.Symbol.Play;
        }
    }
}
