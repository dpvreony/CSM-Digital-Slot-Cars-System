using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using Microsoft.Toolkit.Uwp.UI.Controls;

using SlotCarsGo.Models;
using SlotCarsGo.Services;
using Windows.UI.Xaml.Navigation;
using SlotCarsGo.Models.Racing;
using GalaSoft.MvvmLight.Views;
using SlotCarsGo.Models.Manager;

namespace SlotCarsGo.ViewModels
{
    public class GridSetupViewModel : NavigableViewModelBase
    {
        private RaceTypeBase raceType;
        private User _selected;

        // fields for display
        // title (race type)
        // list of player names
        // list of player images
        // list of selected cars

        // Static list of position IDs, but hide if not used?
        // some kind of popup to see car image
        // OK button
        // refresh button - spk to server


        public User Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<User> LoggedInUsers { get; private set; } = new ObservableCollection<User>();

        public GridSetupViewModel()
        {
        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            LoggedInUsers.Clear();

            var data = await LoggedInUsersDataService.GetLoggedInUsersAsync();

            foreach (var item in data)
            {
                LoggedInUsers.Add(item);
            }

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = LoggedInUsers.First();
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter != null)
            {
                this.raceType = parameter as RaceTypeBase;
            }

            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        public override void OnNavigatedFrom()
        {
        }
    }
}
