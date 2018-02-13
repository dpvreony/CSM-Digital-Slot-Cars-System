namespace SlotCarsGo.Models.Racing
{
    class FreePlayRace : RaceTypeBase
    {
        public FreePlayRace(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
            : base(raceLimitValue, lapsNotDuration, fuelEnabled)
        {
            this.raceTypeEnum = Helpers.Enums.RaceTypesEnum.FreePlay;
            this.name = "Free Play";
            this.rules = "Players drive for the fun of it - no limit and no rules!";
            this.symbol = Windows.UI.Xaml.Controls.Symbol.Play;
        }
    }
}
