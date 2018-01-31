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

namespace SlotCarsGo.ViewModels
{
    public class RaceHUDViewModel : NavigableViewModelBase
    {
        private RaceSession session;
        private DispatcherTimer raceTimeDisplayTimer;
        private string raceTimeDisplay;
        private string startButtonText = "START";

        // Backing fields for players 1-6 lap time details
        private string player1_BestLap = "00:00.00", player2_BestLap = "00:00.00", player3_BestLap = "00:00.00", player4_BestLap = "00:00.00", player5_BestLap = "00:00.00", player6_BestLap = "00:00.00";
        private string player1_LastLap = "00:00.00", player2_LastLap = "00:00.00", player3_LastLap = "00:00.00", player4_LastLap = "00:00.00", player5_LastLap = "00:00.00", player6_LastLap = "00:00.00";
        private string player1_Diff = "00.00", player2_Diff = "00.00", player3_Diff = "00.00", player4_Diff = "00.00", player5_Diff = "00.00", player6_Diff = "00.00";

        public RaceHUDViewModel()
        {
        }

        internal RaceSession Session => session;
        public string Title => AppManager.Track.Name;
        public string RaceTypeTitle => this.Session.RaceType.Name;
        public string StartTime => this.Session.StartTime.ToString("HH:mm");
        public string StartButtonText { get => this.startButtonText; set => Set("StartButtonText", ref startButtonText, value, true); }


    //        public string Remaining => this.session.RaceType.LapsNotDuration ? $"{this.session.RaceType.RaceLimitValue} Laps" : this.session.RaceType.RaceLength. - (DateTime.Now - this.session.StartTime;


        public string Player1_GridNumber => this.Session.NumberOfPlayers >= 1 ? "1" : String.Empty;
        public string Player2_GridNumber => this.Session.NumberOfPlayers >= 2 ? "2" : String.Empty;
        public string Player3_GridNumber => this.Session.NumberOfPlayers >= 3 ? "3" : String.Empty;
        public string Player4_GridNumber => this.Session.NumberOfPlayers >= 4 ? "4" : String.Empty;
        public string Player5_GridNumber => this.Session.NumberOfPlayers >= 5 ? "5" : String.Empty;
        public string Player6_GridNumber => this.Session.NumberOfPlayers >= 6 ? "6" : String.Empty;
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
        public string BestLapHeaderText1 => this.Session.NumberOfPlayers >= 1 ? "Best Lap" : String.Empty;
        public string BestLapHeaderText2 => this.Session.NumberOfPlayers >= 2 ? "Best Lap" : String.Empty;
        public string BestLapHeaderText3 => this.Session.NumberOfPlayers >= 3 ? "Best Lap" : String.Empty;
        public string BestLapHeaderText4 => this.Session.NumberOfPlayers >= 4 ? "Best Lap" : String.Empty;
        public string BestLapHeaderText5 => this.Session.NumberOfPlayers >= 5 ? "Best Lap" : String.Empty;
        public string BestLapHeaderText6 => this.Session.NumberOfPlayers >= 6 ? "Best Lap" : String.Empty;
        public string LastLapHeaderText1 => this.Session.NumberOfPlayers >= 1 ? "Last Lap" : String.Empty;
        public string LastLapHeaderText2 => this.Session.NumberOfPlayers >= 2 ? "Last Lap" : String.Empty;
        public string LastLapHeaderText3 => this.Session.NumberOfPlayers >= 3 ? "Last Lap" : String.Empty;
        public string LastLapHeaderText4 => this.Session.NumberOfPlayers >= 4 ? "Last Lap" : String.Empty;
        public string LastLapHeaderText5 => this.Session.NumberOfPlayers >= 5 ? "Last Lap" : String.Empty;
        public string LastLapHeaderText6 => this.Session.NumberOfPlayers >= 6 ? "Last Lap" : String.Empty;
        public string DiffHeaderText1 => this.Session.NumberOfPlayers >= 1 ? "Difference" : String.Empty;
        public string DiffHeaderText2 => this.Session.NumberOfPlayers >= 2 ? "Difference" : String.Empty;
        public string DiffHeaderText3 => this.Session.NumberOfPlayers >= 3 ? "Difference" : String.Empty;
        public string DiffHeaderText4 => this.Session.NumberOfPlayers >= 4 ? "Difference" : String.Empty;
        public string DiffHeaderText5 => this.Session.NumberOfPlayers >= 5 ? "Difference" : String.Empty;
        public string DiffHeaderText6 => this.Session.NumberOfPlayers >= 6 ? "Difference" : String.Empty;
        
        public string Player1_BestLap { get => this.Session.NumberOfPlayers >= 1 ? this.player1_BestLap : String.Empty; set => Set(ref player1_BestLap, value);}
        public string Player2_BestLap { get => this.Session.NumberOfPlayers >= 2 ? this.player2_BestLap : String.Empty; set => Set(ref player2_BestLap, value); }
        public string Player3_BestLap { get => this.Session.NumberOfPlayers >= 3 ? this.player3_BestLap : String.Empty; set => Set(ref player3_BestLap, value); }
        public string Player4_BestLap { get => this.Session.NumberOfPlayers >= 4 ? this.player4_BestLap : String.Empty; set => Set(ref player4_BestLap, value); }
        public string Player5_BestLap { get => this.Session.NumberOfPlayers >= 5 ? this.player5_BestLap : String.Empty; set => Set(ref player5_BestLap, value); }
        public string Player6_BestLap { get => this.Session.NumberOfPlayers >= 6 ? this.player6_BestLap : String.Empty; set => Set(ref player6_BestLap, value); }
        public string Player1_LastLap { get => this.Session.NumberOfPlayers >= 1 ? this.player1_LastLap : String.Empty; set => Set(ref player1_LastLap, value); }
        public string Player2_LastLap { get => this.Session.NumberOfPlayers >= 2 ? this.player2_LastLap : String.Empty; set => Set(ref player2_LastLap, value); }
        public string Player3_LastLap { get => this.Session.NumberOfPlayers >= 3 ? this.player3_LastLap : String.Empty; set => Set(ref player3_LastLap, value); }
        public string Player4_LastLap { get => this.Session.NumberOfPlayers >= 4 ? this.player4_LastLap : String.Empty; set => Set(ref player4_LastLap, value); }
        public string Player5_LastLap { get => this.Session.NumberOfPlayers >= 5 ? this.player5_LastLap : String.Empty; set => Set(ref player5_LastLap, value); }
        public string Player6_LastLap { get => this.Session.NumberOfPlayers >= 6 ? this.player6_LastLap : String.Empty; set => Set(ref player6_LastLap, value); }
        public string Player1_Diff { get => this.Session.NumberOfPlayers >= 1 ? this.player1_Diff : String.Empty; set => Set(ref player1_Diff, value); }
        public string Player2_Diff { get => this.Session.NumberOfPlayers >= 2 ? this.player2_Diff : String.Empty; set => Set(ref player2_Diff, value); }
        public string Player3_Diff { get => this.Session.NumberOfPlayers >= 3 ? this.player3_Diff : String.Empty; set => Set(ref player3_Diff, value); }
        public string Player4_Diff { get => this.Session.NumberOfPlayers >= 4 ? this.player4_Diff : String.Empty; set => Set(ref player4_Diff, value); }
        public string Player5_Diff { get => this.Session.NumberOfPlayers >= 5 ? this.player5_Diff : String.Empty; set => Set(ref player5_Diff, value); }
        public string Player6_Diff { get => this.Session.NumberOfPlayers >= 6 ? this.player6_Diff : String.Empty; set => Set(ref player6_Diff, value); }

        public string RaceTimeDisplay { get => raceTimeDisplay; set => Set(ref raceTimeDisplay, value); }

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

        internal void StartButtonClicked()
        {
            // Setup race display timer
            this.RaceTimeDisplay = "00:00:00";

            for (int i=3; i>0; i--)
            {
                this.StartButtonText = i.ToString();
                Task.Delay(1000).Wait();
            }
            this.StartButtonText = "GO!";

            this.Session.RaceStart();
            this.raceTimeDisplayTimer = new DispatcherTimer();
            this.raceTimeDisplayTimer.Tick += RaceTimeDisplayTimer_Tick;
            this.raceTimeDisplayTimer.Interval = new TimeSpan(0, 0, 1);
            this.raceTimeDisplayTimer.Start();
        }

        internal void StopButtonClicked()
        {
            this.raceTimeDisplayTimer.Stop();
            this.StartButtonText = "END";
            this.Session.RaceFinish();
            // TODO: Navigate to results?
        }


        /// <summary>
        /// Event handler for displaying the race time duration from DispatcherTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaceTimeDisplayTimer_Tick(object sender, object e)
        {

            TimeSpan span = DateTime.Now - this.Session.StartTime;
            //            this.RaceTimeDisplay = $"{span.Minutes}:{span.Seconds}.{span.Milliseconds}";
            this.RaceTimeDisplay = span.ToString("mm':'ss'.'ff");

            // For race length, calculate finish time somewhere
            // Do i need to control the race? Isnt powerbase doing that?
            // Just need to display time, and stop shwoing new time after race finish?
            // Who navigates to race results? I'm ok with VM doing the hard work.

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
                        Player1_LastLap = this.Session.DriversPreviousLapTime[0].ToString("mm':'ss'.'fff");
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
            this.session = parameter as RaceSession;
            
            // Call powerbase, start race, countdown on screen? AND GO!!
            // No, as HUD has a start button
            return Task.CompletedTask;
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
