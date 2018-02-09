using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;
using SlotCarsGo.Models.Racing;
using SlotCarsGo.Models.Manager;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Models.Comms;
using GalaSoft.MvvmLight.Threading;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Threading;
using SlotCarsGo.Services;
using Windows.UI.Xaml.Controls;
using SlotCarsGo.Views;
using Windows.UI.Xaml.Media;

namespace SlotCarsGo.ViewModels
{
    public class RaceHUDViewModel : NavigableViewModelBase
    {
        private Page raceGridPage;
        private RaceSession session;
        private DispatcherTimer countdownDisplayTimer;
        private DispatcherTimer raceTimeDisplayTimer;
        private string raceTimeDisplay = "00:00:00.0";
        private string remainingDisplay;
        private int countdown = 4;
        private string raceButtonText = "START";
        private SolidColorBrush greenBrush = new SolidColorBrush(Windows.UI.Colors.LimeGreen);
        private SolidColorBrush amberBrush = new SolidColorBrush(Windows.UI.Colors.Orange);
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
        private SolidColorBrush raceButtonBrush;
        private const string EmptyLapTime = "0:00.00";
        private const string EmptyDiffTime = "00.00";
        // Backing fields for players 1-6 lap time details
        private string player1_BestLap = EmptyLapTime, player2_BestLap = EmptyLapTime, player3_BestLap = EmptyLapTime, player4_BestLap = EmptyLapTime, player5_BestLap = EmptyLapTime, player6_BestLap = EmptyLapTime;
        private string player1_LastLap = EmptyLapTime, player2_LastLap = EmptyLapTime, player3_LastLap = EmptyLapTime, player4_LastLap = EmptyLapTime, player5_LastLap = EmptyLapTime, player6_LastLap = EmptyLapTime;
        private string player1_Diff = EmptyDiffTime, player2_Diff = EmptyDiffTime, player3_Diff = EmptyDiffTime, player4_Diff = EmptyDiffTime, player5_Diff = EmptyDiffTime, player6_Diff = EmptyDiffTime;


        public RaceHUDViewModel()
        {
            this.RaceButtonBrush = this.greenBrush;
        }

        internal RaceSession Session => session;
        public string Title => AppManager.Track.Name;
        public string RaceTypeTitle => this.Session.RaceType.Name;
        public string StartTime => this.Session.StartTime.ToString("HH:mm");
        public string RaceButtonText { get => this.raceButtonText; set => Set(ref raceButtonText, value); }
        public string RaceTimeDisplay { get => raceTimeDisplay; set => Set(ref raceTimeDisplay, value); }
        public string RemainingDisplay { get => remainingDisplay; set => Set(ref remainingDisplay, value); }
        public Page RaceGridPage { get => raceGridPage; set => raceGridPage = value; }
        public SolidColorBrush RaceButtonBrush { get => raceButtonBrush; set => Set(ref raceButtonBrush, value); }

        public string Player1_GridNumber => "1";
        public string Player2_GridNumber => "2";
        public string Player3_GridNumber => "3";
        public string Player4_GridNumber => "4";
        public string Player5_GridNumber => "5";
        public string Player6_GridNumber => "6";
        public string Player1_Avatar => this.Session.NumberOfPlayers >= 1 ? this.Session.Players[0].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player2_Avatar => this.Session.NumberOfPlayers >= 2 ? this.Session.Players[1].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player3_Avatar => this.Session.NumberOfPlayers >= 3 ? this.Session.Players[2].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player4_Avatar => this.Session.NumberOfPlayers >= 4 ? this.Session.Players[3].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player5_Avatar => this.Session.NumberOfPlayers >= 5 ? this.Session.Players[4].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player6_Avatar => this.Session.NumberOfPlayers >= 6 ? this.Session.Players[5].User.AvatarSource : User.DefaultUser.AvatarSource;
        public string Player1_Name => this.Session.NumberOfPlayers >= 1 ? this.Session.Players[0].User.Nickname : String.Empty;
        public string Player2_Name => this.Session.NumberOfPlayers >= 2 ? this.Session.Players[1].User.Nickname : String.Empty;
        public string Player3_Name => this.Session.NumberOfPlayers >= 3 ? this.Session.Players[2].User.Nickname : String.Empty;
        public string Player4_Name => this.Session.NumberOfPlayers >= 4 ? this.Session.Players[3].User.Nickname : String.Empty;
        public string Player5_Name => this.Session.NumberOfPlayers >= 5 ? this.Session.Players[4].User.Nickname : String.Empty;
        public string Player6_Name => this.Session.NumberOfPlayers >= 6 ? this.Session.Players[5].User.Nickname : String.Empty;

        public string BestLapHeaderText => "Best Lap";
        public string LastLapHeaderText => "Last Lap";
        public string DiffHeaderText => "Difference";
        
        public string Player1_BestLap { get => this.player1_BestLap; set => Set(ref player1_BestLap, value);}
        public string Player2_BestLap { get => this.player2_BestLap; set => Set(ref player2_BestLap, value); }
        public string Player3_BestLap { get => this.player3_BestLap; set => Set(ref player3_BestLap, value); }
        public string Player4_BestLap { get => this.player4_BestLap; set => Set(ref player4_BestLap, value); }
        public string Player5_BestLap { get => this.player5_BestLap; set => Set(ref player5_BestLap, value); }
        public string Player6_BestLap { get => this.player6_BestLap; set => Set(ref player6_BestLap, value); }
        public string Player1_LastLap
        {
            get => this.player1_LastLap;
            set
            {
                Set(ref player1_LastLap, value);

            }
        }
        public string Player2_LastLap { get => this.player2_LastLap; set => Set(ref player2_LastLap, value); }
        public string Player3_LastLap { get => this.player3_LastLap; set => Set(ref player3_LastLap, value); }
        public string Player4_LastLap { get => this.player4_LastLap; set => Set(ref player4_LastLap, value); }
        public string Player5_LastLap { get => this.player5_LastLap; set => Set(ref player5_LastLap, value); }
        public string Player6_LastLap { get => this.player6_LastLap; set => Set(ref player6_LastLap, value); }
        public string Player1_Diff { get => this.player1_Diff; set => Set(ref player1_Diff, value); }
        public string Player2_Diff { get => this.player2_Diff; set => Set(ref player2_Diff, value); }
        public string Player3_Diff { get => this.player3_Diff; set => Set(ref player3_Diff, value); }
        public string Player4_Diff { get => this.player4_Diff; set => Set(ref player4_Diff, value); }
        public string Player5_Diff { get => this.player5_Diff; set => Set(ref player5_Diff, value); }
        public string Player6_Diff { get => this.player6_Diff; set => Set(ref player6_Diff, value); }


        /*
                public string Player1_BestLap { get => this.Session.NumberOfPlayers >= 1 ? "03.316" : String.Empty; set => Set(ref player1_BestLap, value); }
                public string Player2_BestLap { get => this.Session.NumberOfPlayers >= 2 ? "04.576" : String.Empty; set => Set(ref player2_BestLap, value); }
                public string Player3_BestLap { get => this.Session.NumberOfPlayers >= 3 ? "03.812" : String.Empty; set => Set(ref player3_BestLap, value); }
                public string Player4_BestLap { get => this.Session.NumberOfPlayers >= 4 ? "05.043" : String.Empty; set => Set(ref player4_BestLap, value); }
                public string Player5_BestLap { get => this.Session.NumberOfPlayers >= 5 ? "06.542" : String.Empty; set => Set(ref player5_BestLap, value); }
                public string Player6_BestLap { get => this.Session.NumberOfPlayers >= 6 ? "03.986" : String.Empty; set => Set(ref player6_BestLap, value); }


                public string Player1_LastLap { get => this.Session.NumberOfPlayers >= 1 ? "03.786" : String.Empty; set => Set(ref player1_LastLap, value); }
                public string Player2_LastLap { get => this.Session.NumberOfPlayers >= 2 ? "05.213" : String.Empty; set => Set(ref player2_LastLap, value); }
                public string Player3_LastLap { get => this.Session.NumberOfPlayers >= 3 ? "04.265" : String.Empty; set => Set(ref player3_LastLap, value); }
                public string Player4_LastLap { get => this.Session.NumberOfPlayers >= 4 ? "07.253" : String.Empty; set => Set(ref player4_LastLap, value); }
                public string Player5_LastLap { get => this.Session.NumberOfPlayers >= 5 ? "08.386" : String.Empty; set => Set(ref player5_LastLap, value); }
                public string Player6_LastLap { get => this.Session.NumberOfPlayers >= 6 ? "04.034" : String.Empty; set => Set(ref player6_LastLap, value); }
                public string Player1_Diff { get => this.Session.NumberOfPlayers >= 1 ? "00.470" : String.Empty; set => Set(ref player1_Diff, value); }
                public string Player2_Diff { get => this.Session.NumberOfPlayers >= 2 ? "00.637" : String.Empty; set => Set(ref player2_Diff, value); }
                public string Player3_Diff { get => this.Session.NumberOfPlayers >= 3 ? "00.453" : String.Empty; set => Set(ref player3_Diff, value); }
                public string Player4_Diff { get => this.Session.NumberOfPlayers >= 4 ? "02.210" : String.Empty; set => Set(ref player4_Diff, value); }
                public string Player5_Diff { get => this.Session.NumberOfPlayers >= 5 ? "01.844" : String.Empty; set => Set(ref player5_Diff, value); }
                public string Player6_Diff { get => this.Session.NumberOfPlayers >= 6 ? "00.048" : String.Empty; set => Set(ref player6_Diff, value); }
        */

        /// <summary>
        /// Event handler for the start button, starts a race timer and starts the Powerbase communications.
        /// </summary>
        internal void RaceButtonClicked()
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString()}: StartButton clicked.");

            if (this.Session.Started)
            {
                this.raceTimeDisplayTimer.Stop();
                this.Session.ResetRace();
                this.RaceButtonBrush = this.greenBrush;
                this.RaceTimeDisplay = "00:00:00.0";
                this.RaceButtonText = "START";
                this.Player1_BestLap = EmptyLapTime;
                this.Player2_BestLap = EmptyLapTime;
                this.Player3_BestLap = EmptyLapTime;
                this.Player4_BestLap = EmptyLapTime;
                this.Player5_BestLap = EmptyLapTime;
                this.Player6_BestLap = EmptyLapTime;
                this.Player1_LastLap = EmptyLapTime;
                this.Player2_LastLap = EmptyLapTime;
                this.Player3_LastLap = EmptyLapTime;
                this.Player4_LastLap = EmptyLapTime;
                this.Player5_LastLap = EmptyLapTime;
                this.Player6_LastLap = EmptyLapTime;
                this.Player1_Diff = EmptyDiffTime;
                this.Player2_Diff = EmptyDiffTime;
                this.Player3_Diff = EmptyDiffTime;
                this.Player4_Diff = EmptyDiffTime;
                this.Player5_Diff = EmptyDiffTime;
                this.Player6_Diff = EmptyDiffTime;
            }
            else
            {
                this.RaceButtonBrush = this.amberBrush;

                // Setup countdown display timer
                this.countdown = 4;
                this.countdownDisplayTimer = new DispatcherTimer();
                this.countdownDisplayTimer.Tick += CountdownDisplayTimer_Tick;
                this.countdownDisplayTimer.Interval = new TimeSpan(0, 0, 1);
                this.countdownDisplayTimer.Start();
            }
        }

        /// <summary>
        /// Event handler for the quit race button,navigates to the Main (home) screen which stops the powerbase.
        /// </summary>
        internal void QuitButtonClicked()
        {
            if (this.raceTimeDisplayTimer != null)
            {
                this.raceTimeDisplayTimer.Stop();
            }
            this.RaceButtonText = "END";
            this.Session.QuitRace();
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
        }

        /// <summary>
        /// Event handler for displaying the race time duration from DispatcherTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountdownDisplayTimer_Tick(object sender, object e)
        {
            if (--this.countdown == 0)
            {
                this.countdownDisplayTimer.Stop();
                this.RaceButtonBrush = this.redBrush;
                this.RaceButtonText = "RESET";
                this.Session.StartRace();
                SimpleIoc.Default.GetInstance<Powerbase>().ResetGameTimer();

                // Setup race display timer
                this.raceTimeDisplayTimer = new DispatcherTimer();
                this.raceTimeDisplayTimer.Tick += RaceTimeDisplayTimer_Tick;
                this.raceTimeDisplayTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                this.raceTimeDisplayTimer.Start();
            }
            else
            {
                this.RaceButtonText = this.countdown.ToString();
            }
        }

        /// <summary>
        /// Event handler for displaying the race time duration from DispatcherTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaceTimeDisplayTimer_Tick(object sender, object e)
        {
            TimeSpan span = DateTime.Now - this.Session.StartTime;
            this.RaceTimeDisplay = span.ToString("hh':'mm':'ss'.'f");

            if (this.Session.RaceType.LapsNotDuration)
            {
                this.RemainingDisplay = $"{this.session.LapsRemaining} / {this.session.RaceType.RaceLimitValue} Laps";
            }
            else
            {
                TimeSpan remaining = this.Session.RaceType.RaceLength - span;
                this.RemainingDisplay = remaining.ToString("hh':'mm':'ss'.'f");
            }
        }

        public void RaceFinished()
        {
            // TODO: create a routine that drives all cars to finish line! (before closing PB)
            // TODO: check that data is saved before ending

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(RaceResultsViewModel).FullName, this.Session);
            });
        }


        /// <summary>
        /// Called by race session to update a given players lap times when crossed the finish line.
        /// </summary>
        /// <param name="carId">The player to refresh (1 index).</param>
        internal void UpdateLapTimes(int carId)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                // Dispatch back to the main thread
                switch (carId)
                {
                    case 0:
                        Player1_BestLap = this.Session.DriversFastestLapTimes[0].ToString("m':'ss'.'fff");
                        Player1_LastLap = this.Session.DriversPreviousLapTime[0].ToString("m':'ss'.'fff");
                        Player1_Diff = (this.Session.DriversPreviousLapTime[0] - this.Session.DriversFastestLapTimes[0]).ToString(@"ss\.fff");
                        break;
                    case 1:
                        Player2_BestLap = this.Session.DriversFastestLapTimes[1].ToString("m':'ss'.'fff");
                        Player2_LastLap = this.Session.DriversPreviousLapTime[1].ToString("m':'ss'.'fff");
                        Player2_Diff = (this.Session.DriversPreviousLapTime[1] - this.Session.DriversFastestLapTimes[1]).ToString(@"ss\.fff");
                        break;
                    case 2:
                        Player3_BestLap = this.Session.DriversFastestLapTimes[2].ToString("m':'ss'.'fff");
                        Player3_LastLap = this.Session.DriversPreviousLapTime[2].ToString("m':'ss'.'fff");
                        Player3_Diff = (this.Session.DriversPreviousLapTime[2] - this.Session.DriversFastestLapTimes[2]).ToString(@"ss\.fff");
                        break;
                    case 3:
                        Player4_BestLap = this.Session.DriversFastestLapTimes[3].ToString("m':'ss'.'fff");
                        Player4_LastLap = this.Session.DriversPreviousLapTime[3].ToString("m':'ss'.'fff");
                        Player4_Diff = (this.Session.DriversPreviousLapTime[3] - this.Session.DriversFastestLapTimes[3]).ToString(@"ss\.fff");
                        break;
                    case 4:
                        Player5_BestLap = this.Session.DriversFastestLapTimes[4].ToString("m':'ss'.'fff");
                        Player5_LastLap = this.Session.DriversPreviousLapTime[4].ToString("m':'ss'.'fff");
                        Player5_Diff = (this.Session.DriversPreviousLapTime[4] - this.Session.DriversFastestLapTimes[4]).ToString(@"ss\.fff");
                        break;
                    case 5:
                        Player6_BestLap = this.Session.DriversFastestLapTimes[5].ToString("m':'ss'.'fff");
                        Player6_LastLap = this.Session.DriversPreviousLapTime[5].ToString("m':'ss'.'fff");
                        Player6_Diff = (this.Session.DriversPreviousLapTime[5] - this.Session.DriversFastestLapTimes[5]).ToString(@"ss\.fff");
                        break;
                    default:
                        break;
                }
            });
        }

        public override void OnNavigatedFrom()
        {
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (!SimpleIoc.Default.GetInstance<Powerbase>().IsPowerbaseConnected)
            {
                SimpleIoc.Default.GetInstance<Powerbase>().Listen();
            }

            this.session = parameter as RaceSession;
            switch (this.Session.NumberOfPlayers)
            {
                case 1:
                    RaceGridPage = new RaceGridPanelFor1Player();
                    break;
                case 2:
                    RaceGridPage = new RaceGridPanelFor2Players();
                    break;
                case 3:
                    RaceGridPage = new RaceGridPanelFor3Players();
                    break;
                case 4:
                    RaceGridPage = new RaceGridPanelFor4Players();
                    break;
                case 5:
                    RaceGridPage = new RaceGridPanelFor5Players();
                    break;
                case 6:
                    RaceGridPage = new RaceGridPanelFor6Players();
                    break;
                default:
                    RaceGridPage = new RaceGridPanelFor6Players();
                    break;
            }

            if (this.Session.RaceType.LapsNotDuration)
            {
                this.RemainingDisplay = $"{this.session.LapsRemaining} / {this.session.RaceType.RaceLimitValue} Laps";
            }
            else
            {
                this.RemainingDisplay = this.Session.RaceType.RaceLength.ToString("hh':'mm':'ss'.'f");
            }

            SimpleIoc.Default.GetInstance<Powerbase>().UpdateRaceSession(this.session);

            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!this.Session.Finished)
            {
                this.Session.Finished = true;
            }

            SimpleIoc.Default.GetInstance<Powerbase>().StopListening();
        }
    }
}
