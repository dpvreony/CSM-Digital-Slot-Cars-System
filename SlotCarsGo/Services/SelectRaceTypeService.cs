using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using SlotCarsGo.Models.Racing;
using static SlotCarsGo.Helpers.Enums;

namespace SlotCarsGo.Services
{
    public static class SelectRaceTypeService
    {
        private static IEnumerable<RaceType> AllRaceTypes()
        {
            // The following is order summary data
            var data = new ObservableCollection<RaceType>
            {
                new RaceType(
                    RaceTypesEnum.FreePlay,
                    "Free Play",
                    "Players drive for the fun of it - no limit and no rules!",
                    999,
                    false,
                    false,
                    0,
                    Symbol.Play),

                new RaceType(
                    RaceTypesEnum.Qualifying,
                    "Qualifying",
                    "Players race one at a time or all together to record the fastest lap in the session. The results can be used to decide the grid order of a Grand Prix race.",
                    10,
                    true,
                    false,
                    0,
                    Symbol.FourBars),

                new RaceType(
                    RaceTypesEnum.GP,
                    "Grand Prix",
                    "Multiple players race against each other to complete the set number of laps in the fastest time or the most laps in the set number of minutes.",
                    30,
                    true,
                    true,
                    0,
                    Symbol.Flag),

                new RaceType(
                    RaceTypesEnum.Timetrial,
                    "Time Trial",
                    "Players race one at a time in a series to complete the set number of laps in the shortest time.",
                    10,
                    true,
                    false,
                    0,
                    Symbol.RepeatAll)
            };

            return data;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<RaceType>> GetRaceTypesDataAsync()
        {
            await Task.CompletedTask;

            return AllRaceTypes();
        }
    }
}
