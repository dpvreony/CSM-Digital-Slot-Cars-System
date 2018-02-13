using System;
using Windows.UI.Xaml.Controls;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Models.Racing
{
    public abstract class RaceTypeBase
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

        /// <summary>
        /// Base constructor for race type base class.
        /// </summary>
        public RaceTypeBase(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
        {
            this.raceLimitValue = raceLimitValue;
            this.raceLength = new TimeSpan(0, raceLimitValue, 0);
            this.lapsNotDuration = lapsNotDuration;
            this.fuelEnabled = fuelEnabled;
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
