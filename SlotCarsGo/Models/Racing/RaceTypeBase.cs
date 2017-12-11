using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Models.Racing
{
    public abstract class RaceTypeBase
    {
        internal string Name { get; set; }
        internal string Rules { get; set; }
        internal int RaceLimitValue { get; set; }
        internal TimeSpan RaceLength { get; set; }
        internal bool LapsNotDuration { get; set; }
        internal bool FuelEnabled { get; set; }
        internal Int32 CrashPenalty { get; set; }
        public Symbol Symbol { get; set; }

        /// <summary>
        /// Base constructor for race type base class.
        /// </summary>
        public RaceTypeBase(int raceLimitValue, bool lapsNotDuration, bool fuelEnabled)
        {
            this.RaceLimitValue = raceLimitValue;
            this.RaceLength = new TimeSpan(0, raceLimitValue, 0);
            this.LapsNotDuration = lapsNotDuration;
            this.FuelEnabled = fuelEnabled;
        }

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
            return $"{Name} Race";
        }
    }
}
