using System;

using GalaSoft.MvvmLight;
using SlotCarsGo.Models.Racing;
using SlotCarsGo.Models.Manager;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SlotCarsGo.Services;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Models.Comms;

namespace SlotCarsGo.ViewModels
{
    public class GridConfirmationViewModel : NavigableViewModelBase
    {
        public GridConfirmationViewModel()
        {
        }

        private RaceTypeBase raceType;
        private User _selected;

        public User Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        /// <summary>
        /// Removes the user from the list of logged in users and from the race session.
        /// </summary>
        /// <param name="userIndexInGrid">The index in loggedInUsers to remove from.</param>
        internal void RemoveLoggedInUser(int userIndexInGrid)
        {
            try
            {
                // USE DispatchHelper.CheckBeginInvokeOnUI
                this.LoggedInUsers.RemoveAt(userIndexInGrid);
                // TODO: send message to server/client that theyve been removed



            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Could not remove user from grid: {e.Message}.");
            }
        }

        public ObservableCollection<User> LoggedInUsers { get; private set; } = new ObservableCollection<User>();

        public string Title => raceType != null ? raceType.Name : String.Empty;
        public string Player1_GridNumber => LoggedInUsers.Count >= 1 ? "1" : String.Empty;
        public string Player2_GridNumber => LoggedInUsers.Count >= 2 ? "2" : String.Empty;
        public string Player3_GridNumber => LoggedInUsers.Count >= 3 ? "3" : String.Empty;
        public string Player4_GridNumber => LoggedInUsers.Count >= 4 ? "4" : String.Empty;
        public string Player5_GridNumber => LoggedInUsers.Count >= 5 ? "5" : String.Empty;
        public string Player6_GridNumber => LoggedInUsers.Count >= 6 ? "6" : String.Empty;
        public string Player1_Avatar => LoggedInUsers.Count >= 1 ? LoggedInUsers[0].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player2_Avatar => LoggedInUsers.Count >= 2 ? LoggedInUsers[1].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player3_Avatar => LoggedInUsers.Count >= 3 ? LoggedInUsers[2].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player4_Avatar => LoggedInUsers.Count >= 4 ? LoggedInUsers[3].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player5_Avatar => LoggedInUsers.Count >= 5 ? LoggedInUsers[4].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player6_Avatar => LoggedInUsers.Count >= 6 ? LoggedInUsers[5].AvatarSource : User.DefaultUser.AvatarSource;
        public string Player1_Name => LoggedInUsers.Count >= 1 ? LoggedInUsers[0].Nickname : String.Empty;
        public string Player2_Name => LoggedInUsers.Count >= 2 ? LoggedInUsers[1].Nickname : String.Empty;
        public string Player3_Name => LoggedInUsers.Count >= 3 ? LoggedInUsers[2].Nickname : String.Empty;
        public string Player4_Name => LoggedInUsers.Count >= 4 ? LoggedInUsers[3].Nickname : String.Empty;
        public string Player5_Name => LoggedInUsers.Count >= 5 ? LoggedInUsers[4].Nickname : String.Empty;
        public string Player6_Name => LoggedInUsers.Count >= 6 ? LoggedInUsers[5].Nickname : String.Empty;
        public string Player1_Car => LoggedInUsers.Count >= 1 ? LoggedInUsers[0].SelectedCar.Name : String.Empty;
        public string Player2_Car => LoggedInUsers.Count >= 2 ? LoggedInUsers[1].SelectedCar.Name : String.Empty;
        public string Player3_Car => LoggedInUsers.Count >= 3 ? LoggedInUsers[2].SelectedCar.Name : String.Empty;
        public string Player4_Car => LoggedInUsers.Count >= 4 ? LoggedInUsers[3].SelectedCar.Name : String.Empty;
        public string Player5_Car => LoggedInUsers.Count >= 5 ? LoggedInUsers[4].SelectedCar.Name : String.Empty;
        public string Player6_Car => LoggedInUsers.Count >= 6 ? LoggedInUsers[5].SelectedCar.Name : String.Empty;

        public async void RefreshLoggedInUsers()
        {
            await this.LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            LoggedInUsers.Clear();

            var data = await LoggedInUsersDataService.GetLoggedInUsersAsync();

            foreach (var item in data)
            {
                LoggedInUsers.Add(item);
            }
            Selected = LoggedInUsers.First();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter != null)
            {
                this.raceType = parameter as RaceTypeBase;
            }

            RefreshLoggedInUsers();

            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        public override void OnNavigatedFrom()
        {
        }

        public void ConfirmGridAndProceed()
        {
            RaceSession session = new RaceSession(raceType, LoggedInUsers);
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(RaceHUDViewModel).FullName, session);
        }
    }
}
