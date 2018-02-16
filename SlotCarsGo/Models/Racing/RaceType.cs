using System;
using SlotCarsGo.Helpers;
using Windows.UI.Xaml.Controls;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Models.Racing
{
    public class RaceType
    {
        protected RaceTypesEnum raceTypeEnum;
        protected string name;
        protected string rules;
        protected int raceLimitValue;
        protected TimeSpan raceLength;
        protected bool lapsNotDuration;
        protected bool fuelEnabled;
        protected Int32 crashPenalty;
        protected Symbol symbol;

        public RaceType(RaceTypesEnum raceTypeEnum, string name, string rules, int raceLimitValue, bool lapsNotDuration,
            bool fuelEnabled, int crashPenalty, Symbol symbol)
        {
            this.raceTypeEnum = raceTypeEnum;
            this.name = name;
            this.rules = rules;
            this.raceLimitValue = raceLimitValue;
            this.raceLength = new TimeSpan();
            this.lapsNotDuration = lapsNotDuration;
            this.fuelEnabled = fuelEnabled;
            this.crashPenalty = crashPenalty;
            this.symbol = symbol;
        }

        public RaceTypesEnum RaceTypeEnum { get => raceTypeEnum; }
        public string Name { get => name; }
        public string Rules { get => rules; }
        public int RaceLimitValue { get => raceLimitValue; set => raceLimitValue = value; }
        public TimeSpan RaceLength { get => raceLength; set => raceLength = value; }
        public bool LapsNotDuration { get => lapsNotDuration; set => lapsNotDuration = value; }
        public bool FuelEnabled { get => fuelEnabled; }
        public int CrashPenalty { get => crashPenalty; }
        public Symbol Symbol { get => symbol; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public string HashIdentIcon
        {
            get { return GetHashCode().ToString() + "-icon"; }
        }

        public string HashIdentTitle
        {
            get { return GetHashCode().ToString() + "-title"; }
        }

        public override string ToString()
        {
            return $"{this.Name} Race";
        }
    }
}
