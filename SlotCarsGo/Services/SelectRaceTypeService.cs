using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using SlotCarsGo.Models;

using Windows.UI.Xaml.Controls;
using SlotCarsGo.Models.Racing;

namespace SlotCarsGo.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
    public static class SelectRaceTypeService
    {
        private static IEnumerable<RaceTypeBase> AllRaceTypes()
        {
            // The following is order summary data
            var data = new ObservableCollection<RaceTypeBase>
            {
                //TODO: replace with single class, no need for different classes. Rename RaceTypeBase to racetype
                new FreePlayRace(999, false, false),
                new QualifyingRace(10, true, false),
                new GrandPrixRace(30, true, true),
                new TimeTrialRace(10, true, false)
            };

            return data;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<RaceTypeBase>> GetRaceTypesDataAsync()
        {
            await Task.CompletedTask;

            return AllRaceTypes();
        }
    }
}
