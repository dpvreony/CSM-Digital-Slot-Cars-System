
namespace SlotCarsGo.Models.Racing
{
    class TimeTrialRace : RaceTypeBase
    {

        public TimeTrialRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled) 
            : base (raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.raceTypeEnum = Helpers.Enums.RaceTypesEnum.Timetrial;
            this.name = "Time Trial";
            this.rules = "Players race one at a time in a series to complete the set number of laps in the shortest time.";
            this.symbol = Windows.UI.Xaml.Controls.Symbol.RepeatAll;
        }
    }
}
