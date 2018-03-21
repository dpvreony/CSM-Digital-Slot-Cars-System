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
using Windows.UI.Xaml;
using System.Collections;

namespace SlotCarsGo.ViewModels
{
    public class GridConfirmationViewModel : NavigableViewModelBase
    {
        public GridConfirmationViewModel()
        {
        }

        private RaceType raceType;
        private Driver _selected;
        private ObservableCollection<Driver> loggedInDrivers = new ObservableCollection<Driver>();
        private bool gridConfirmed = false;
        private bool[] playersConfirmed;
        private string player1_GridNumber, player2_GridNumber, player3_GridNumber, player4_GridNumber, player5_GridNumber, player6_GridNumber;
        private string player1_Avatar, player2_Avatar, player3_Avatar, player4_Avatar, player5_Avatar, player6_Avatar;
        private string player1_Name, player2_Name, player3_Name, player4_Name, player5_Name, player6_Name;
        private string player1_Car, player2_Car, player3_Car, player4_Car, player5_Car, player6_Car;
        private string confirmPlayerIcon_Warning = "\xE783";
        private string confirmPlayerIcon_Checked = "\xE930";
        private string removePlayerIcon = "\xF096";
        private SolidColorBrush confirmPlayerIconBrush_OK = new SolidColorBrush(Windows.UI.Colors.LimeGreen);
        private SolidColorBrush confirmPlayerIconBrush_TBC = new SolidColorBrush(Windows.UI.Colors.Gold);
        private string player1_ConfirmGridIcon, player2_ConfirmGridIcon, player3_ConfirmGridIcon, player4_ConfirmGridIcon, player5_ConfirmGridIcon, player6_ConfirmGridIcon;
        private Brush player1_ConfirmGridColour, player2_ConfirmGridColour, player3_ConfirmGridColour, player4_ConfirmGridColour, player5_ConfirmGridColour, player6_ConfirmGridColour;
        private Visibility player1Visibility = Visibility.Collapsed;
        private Visibility player2Visibility = Visibility.Collapsed;
        private Visibility player3Visibility = Visibility.Collapsed;
        private Visibility player4Visibility = Visibility.Collapsed;
        private Visibility player5Visibility = Visibility.Collapsed;
        private Visibility player6Visibility = Visibility.Collapsed;

        public Driver Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Driver> LoggedInDrivers { get => this.loggedInDrivers; set => Set(ref this.loggedInDrivers, value); }
        public string Title => raceType != null ? raceType.Name : String.Empty;
        public string Player1_GridNumber { get => this.LoggedInDrivers.Count >= 1 ? this.LoggedInDrivers[0].ControllerId.ToString() : String.Empty; set => Set(ref player1_GridNumber, value); }
        public string Player2_GridNumber { get => this.LoggedInDrivers.Count >= 2 ? this.LoggedInDrivers[1].ControllerId.ToString() : String.Empty; set => Set(ref player2_GridNumber, value); }
        public string Player3_GridNumber { get => this.LoggedInDrivers.Count >= 3 ? this.LoggedInDrivers[2].ControllerId.ToString() : String.Empty; set => Set(ref player3_GridNumber, value); }
        public string Player4_GridNumber { get => this.LoggedInDrivers.Count >= 4 ? this.LoggedInDrivers[3].ControllerId.ToString() : String.Empty; set => Set(ref player4_GridNumber, value); }
        public string Player5_GridNumber { get => this.LoggedInDrivers.Count >= 5 ? this.LoggedInDrivers[4].ControllerId.ToString() : String.Empty; set => Set(ref player5_GridNumber, value); }
        public string Player6_GridNumber { get => this.LoggedInDrivers.Count >= 6 ? this.LoggedInDrivers[5].ControllerId.ToString() : String.Empty; set => Set(ref player6_GridNumber, value); }
        public string Player1_Avatar { get => this.LoggedInDrivers.Count >= 1 ? this.player1_Avatar : Driver.DefaultImageName; set => Set(ref player1_Avatar, value); }
        public string Player2_Avatar { get => this.LoggedInDrivers.Count >= 2 ? this.player2_Avatar : Driver.DefaultImageName; set => Set(ref player2_Avatar, value); }
        public string Player3_Avatar { get => this.LoggedInDrivers.Count >= 3 ? this.player3_Avatar : Driver.DefaultImageName; set => Set(ref player3_Avatar, value); }
        public string Player4_Avatar { get => this.LoggedInDrivers.Count >= 4 ? this.player4_Avatar : Driver.DefaultImageName; set => Set(ref player4_Avatar, value); }
        public string Player5_Avatar { get => this.LoggedInDrivers.Count >= 5 ? this.player5_Avatar : Driver.DefaultImageName; set => Set(ref player5_Avatar, value); }
        public string Player6_Avatar { get => this.LoggedInDrivers.Count >= 6 ? this.player6_Avatar : Driver.DefaultImageName; set => Set(ref player6_Avatar, value); }
        public string Player1_Name { get => this.LoggedInDrivers.Count >= 1 ? this.LoggedInDrivers[0].UserName : String.Empty; set => Set(ref player1_Name, value); }
        public string Player2_Name { get => this.LoggedInDrivers.Count >= 2 ? this.LoggedInDrivers[1].UserName : String.Empty; set => Set(ref player2_Name, value); }
        public string Player3_Name { get => this.LoggedInDrivers.Count >= 3 ? this.LoggedInDrivers[2].UserName : String.Empty; set => Set(ref player3_Name, value); }
        public string Player4_Name { get => this.LoggedInDrivers.Count >= 4 ? this.LoggedInDrivers[3].UserName : String.Empty; set => Set(ref player4_Name, value); }
        public string Player5_Name { get => this.LoggedInDrivers.Count >= 5 ? this.LoggedInDrivers[4].UserName : String.Empty; set => Set(ref player5_Name, value); }
        public string Player6_Name { get => this.LoggedInDrivers.Count >= 6 ? this.LoggedInDrivers[5].UserName : String.Empty; set => Set(ref player6_Name, value); }
        public string Player1_Car { get => this.LoggedInDrivers.Count >= 1 ? this.LoggedInDrivers[0].SelectedCar.Name : String.Empty; set => Set(ref player1_Car, value); }
        public string Player2_Car { get => this.LoggedInDrivers.Count >= 2 ? this.LoggedInDrivers[1].SelectedCar.Name : String.Empty; set => Set(ref player2_Car, value); }
        public string Player3_Car { get => this.LoggedInDrivers.Count >= 3 ? this.LoggedInDrivers[2].SelectedCar.Name : String.Empty; set => Set(ref player3_Car, value); }
        public string Player4_Car { get => this.LoggedInDrivers.Count >= 4 ? this.LoggedInDrivers[3].SelectedCar.Name : String.Empty; set => Set(ref player4_Car, value); }
        public string Player5_Car { get => this.LoggedInDrivers.Count >= 5 ? this.LoggedInDrivers[4].SelectedCar.Name : String.Empty; set => Set(ref player5_Car, value); }
        public string Player6_Car { get => this.LoggedInDrivers.Count >= 6 ? this.LoggedInDrivers[5].SelectedCar.Name : String.Empty; set => Set(ref player6_Car, value); }
        public Visibility Player1Visibility { get => this.player1Visibility; set => Set(ref player1Visibility, value); }
        public Visibility Player2Visibility { get => this.player2Visibility; set => Set(ref player2Visibility, value); }
        public Visibility Player3Visibility { get => this.player3Visibility; set => Set(ref player3Visibility, value); }
        public Visibility Player4Visibility { get => this.player4Visibility; set => Set(ref player4Visibility, value); }
        public Visibility Player5Visibility { get => this.player5Visibility; set => Set(ref player5Visibility, value); }
        public Visibility Player6Visibility { get => this.player6Visibility; set => Set(ref player6Visibility, value); }
        public string Player1_ConfirmGridIcon { get => this.player1_ConfirmGridIcon; set => Set(ref player1_ConfirmGridIcon, value); }
        public string Player2_ConfirmGridIcon { get => this.player2_ConfirmGridIcon; set => Set(ref player2_ConfirmGridIcon, value); }
        public string Player3_ConfirmGridIcon { get => this.player3_ConfirmGridIcon; set => Set(ref player3_ConfirmGridIcon, value); }
        public string Player4_ConfirmGridIcon { get => this.player4_ConfirmGridIcon; set => Set(ref player4_ConfirmGridIcon, value); }
        public string Player5_ConfirmGridIcon { get => this.player5_ConfirmGridIcon; set => Set(ref player5_ConfirmGridIcon, value); }
        public string Player6_ConfirmGridIcon { get => this.player6_ConfirmGridIcon; set => Set(ref player6_ConfirmGridIcon, value); }
        public Brush Player1_ConfirmGridColour { get => this.player1_ConfirmGridColour; set => Set(ref player1_ConfirmGridColour, value); }
        public Brush Player2_ConfirmGridColour { get => this.player2_ConfirmGridColour; set => Set(ref player2_ConfirmGridColour, value); }
        public Brush Player3_ConfirmGridColour { get => this.player3_ConfirmGridColour; set => Set(ref player3_ConfirmGridColour, value); }
        public Brush Player4_ConfirmGridColour { get => this.player4_ConfirmGridColour; set => Set(ref player4_ConfirmGridColour, value); }
        public Brush Player5_ConfirmGridColour { get => this.player5_ConfirmGridColour; set => Set(ref player5_ConfirmGridColour, value); }
        public Brush Player6_ConfirmGridColour { get => this.player6_ConfirmGridColour; set => Set(ref player6_ConfirmGridColour, value); }
        public bool GridConfirmed { get => this.gridConfirmed; set => Set(ref this.gridConfirmed, value); }
        public string RemovePlayerIcon { get => removePlayerIcon; set => removePlayerIcon = value; }

        /// <summary>
        /// Refresh logged in users button handler.
        /// </summary>
        public async void RefreshLoggedInUsers()
        {
            await this.LoadDataAsync();
        }

        /// <summary>
        /// Initialise a new array of booleans for confirmed players.
        /// </summary>
        /// <param name="loggedInUsers">The list of logged in users on the grid.</param>
        private void RefreshConfirmedPlayers(IList loggedInUsers)
        {
            this.playersConfirmed = new bool[loggedInUsers.Count];
        }
        
        /// <summary>
        /// Reassigns the details for each player after the logged in users list is modified.
        /// </summary>
        private void ResetPlayerDetailsDisplay()
        {
            this.Player1_GridNumber = this.LoggedInDrivers.Count >= 1 ? this.LoggedInDrivers[0].ControllerId.ToString() : String.Empty;
            this.Player2_GridNumber = this.LoggedInDrivers.Count >= 2 ? this.LoggedInDrivers[1].ControllerId.ToString() : String.Empty;
            this.Player3_GridNumber = this.LoggedInDrivers.Count >= 3 ? this.LoggedInDrivers[2].ControllerId.ToString() : String.Empty;
            this.Player4_GridNumber = this.LoggedInDrivers.Count >= 4 ? this.LoggedInDrivers[3].ControllerId.ToString() : String.Empty;
            this.Player5_GridNumber = this.LoggedInDrivers.Count >= 5 ? this.LoggedInDrivers[4].ControllerId.ToString() : String.Empty;
            this.Player6_GridNumber = this.LoggedInDrivers.Count >= 6 ? this.LoggedInDrivers[5].ControllerId.ToString() : String.Empty;
            this.Player1Visibility = this.LoggedInDrivers.Count >= 1 ? Visibility.Visible : Visibility.Collapsed;
            this.Player2Visibility = this.LoggedInDrivers.Count >= 2 ? Visibility.Visible : Visibility.Collapsed;
            this.Player3Visibility = this.LoggedInDrivers.Count >= 3 ? Visibility.Visible : Visibility.Collapsed;
            this.Player4Visibility = this.LoggedInDrivers.Count >= 4 ? Visibility.Visible : Visibility.Collapsed;
            this.Player5Visibility = this.LoggedInDrivers.Count >= 5 ? Visibility.Visible : Visibility.Collapsed;
            this.Player6Visibility = this.LoggedInDrivers.Count >= 6 ? Visibility.Visible : Visibility.Collapsed;
            this.Player1_Avatar = this.LoggedInDrivers.Count >= 1 ? LoggedInDrivers[0].ImageName : Driver.DefaultImageName;
            this.Player2_Avatar = this.LoggedInDrivers.Count >= 2 ? LoggedInDrivers[1].ImageName : Driver.DefaultImageName; 
            this.Player3_Avatar = this.LoggedInDrivers.Count >= 3 ? LoggedInDrivers[2].ImageName : Driver.DefaultImageName; 
            this.Player4_Avatar = this.LoggedInDrivers.Count >= 4 ? LoggedInDrivers[3].ImageName : Driver.DefaultImageName; 
            this.Player5_Avatar = this.LoggedInDrivers.Count >= 5 ? LoggedInDrivers[4].ImageName : Driver.DefaultImageName; 
            this.Player6_Avatar = this.LoggedInDrivers.Count >= 6 ? LoggedInDrivers[5].ImageName : Driver.DefaultImageName; 
            this.Player1_Name = this.LoggedInDrivers.Count >= 1 ? LoggedInDrivers[0].UserName : String.Empty; 
            this.Player2_Name = this.LoggedInDrivers.Count >= 2 ? LoggedInDrivers[1].UserName : String.Empty; 
            this.Player3_Name = this.LoggedInDrivers.Count >= 3 ? LoggedInDrivers[2].UserName : String.Empty; 
            this.Player4_Name = this.LoggedInDrivers.Count >= 4 ? LoggedInDrivers[3].UserName : String.Empty; 
            this.Player5_Name = this.LoggedInDrivers.Count >= 5 ? LoggedInDrivers[4].UserName : String.Empty; 
            this.Player6_Name = this.LoggedInDrivers.Count >= 6 ? LoggedInDrivers[5].UserName : String.Empty; 
            this.Player1_Car = this.LoggedInDrivers.Count >= 1 ? LoggedInDrivers[0].SelectedCar.Name : String.Empty; 
            this.Player2_Car = this.LoggedInDrivers.Count >= 2 ? LoggedInDrivers[1].SelectedCar.Name : String.Empty; 
            this.Player3_Car = this.LoggedInDrivers.Count >= 3 ? LoggedInDrivers[2].SelectedCar.Name : String.Empty; 
            this.Player4_Car = this.LoggedInDrivers.Count >= 4 ? LoggedInDrivers[3].SelectedCar.Name : String.Empty; 
            this.Player5_Car = this.LoggedInDrivers.Count >= 5 ? LoggedInDrivers[4].SelectedCar.Name : String.Empty; 
            this.Player6_Car = this.LoggedInDrivers.Count >= 6 ? LoggedInDrivers[5].SelectedCar.Name : String.Empty;
            this.Player1_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player2_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player3_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player4_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player5_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player6_ConfirmGridIcon = this.confirmPlayerIcon_Warning;
            this.Player1_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
            this.Player2_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
            this.Player3_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
            this.Player4_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
            this.Player5_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
            this.Player6_ConfirmGridColour = this.confirmPlayerIconBrush_TBC;
        }

        /// <summary>
        /// Confirm the setup of this player on the powerbase.
        /// </summary>
        /// <param name="userIndexInGrid"></param>
        internal void ConfirmUserOnGrid(int userIndexInGrid)
        {
            try
            {
                this.playersConfirmed[userIndexInGrid] = true;
                this.ConfirmPlayerOnGridDisplay(userIndexInGrid);

                bool okToProceed = true;
                foreach (bool userFlag in this.playersConfirmed)
                {
                    okToProceed = userFlag ? okToProceed : false;
                }

                this.GridConfirmed = okToProceed;
            }
            catch(IndexOutOfRangeException)
            {
                this.RefreshLoggedInUsers();
            }
        }

        /// <summary>
        /// Update the display of the confirm button for this player to green OK.
        /// </summary>
        /// <param name="player"></param>
        private void ConfirmPlayerOnGridDisplay(int userIndexInGrid)
        {
            switch (userIndexInGrid)
            {
                case 0:
                    this.Player1_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player1_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                case 1:
                    this.Player2_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player2_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                case 2:
                    this.Player3_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player3_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                case 3:
                    this.Player4_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player4_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                case 4:
                    this.Player5_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player5_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                case 5:
                    this.Player6_ConfirmGridIcon = this.confirmPlayerIcon_Checked;
                    this.Player6_ConfirmGridColour = this.confirmPlayerIconBrush_OK;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Removes the user from the list of logged in users and from the race session.
        /// </summary>
        /// <param name="userIndexInGrid">The index in loggedInUsers to remove from.</param>
        internal void RemoveLoggedInUser(int userIndexInGrid)
        {
            try
            {
                this.LoggedInDrivers.RemoveAt(userIndexInGrid);
                this.ResetPlayerDetailsDisplay();
                this.GridConfirmed = false;
                this.RefreshConfirmedPlayers(this.LoggedInDrivers);

                // TODO: send message to server/client that theyve been removed from grid
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Could not remove user from grid: {e.Message}.");
            }
        }

        /// <summary>
        /// Reloads the logged in users.
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            this.LoggedInDrivers.Clear(); 

            var data = await DriversWaitingToRaceDataService.GetDriversWaitingToRaceAsync();

            foreach (var item in data)
            {
                this.LoggedInDrivers.Add(item);
            }

            this.GridConfirmed = false;
            this.RefreshConfirmedPlayers(this.LoggedInDrivers);
            this.ResetPlayerDetailsDisplay();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter == null)
            {
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
            }
            else
            {                        
                this.raceType = parameter as RaceType;
                this.RefreshLoggedInUsers();
            }

            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        public override void OnNavigatedFrom()
        {
        }

        /// <summary>
        /// Navigates to the race HUD page and passes a new session instance.
        /// </summary>
        public void ConfirmGridAndProceed()
        {
            RaceSession session = new RaceSession(this.raceType, this.LoggedInDrivers);
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(RaceHUDViewModel).FullName, session);
        }
    }
}
