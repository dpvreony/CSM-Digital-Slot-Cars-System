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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static SlotCarsGo.Helpers.Enums;
using System.Net.Http;
using System.Net.Http.Headers;
using SlotCarsGo_Server.Models.DTO;
using AutoMapper;
using Newtonsoft.Json;

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
        private TimeSpan zeroTimeSpan = new TimeSpan(0);
        private CancellationTokenSource checkForRaceFinishCancellationTokenSource;
        // Backing fields for players 1-6 lap time details
        private string player1_GridNumber, player2_GridNumber, player3_GridNumber, player4_GridNumber, player5_GridNumber, player6_GridNumber;
        private string player1_BestLap = EmptyLapTime, player2_BestLap = EmptyLapTime, player3_BestLap = EmptyLapTime, player4_BestLap = EmptyLapTime, player5_BestLap = EmptyLapTime, player6_BestLap = EmptyLapTime;
        private string player1_LastLap = EmptyLapTime, player2_LastLap = EmptyLapTime, player3_LastLap = EmptyLapTime, player4_LastLap = EmptyLapTime, player5_LastLap = EmptyLapTime, player6_LastLap = EmptyLapTime;
        private string player1_Diff = EmptyDiffTime, player2_Diff = EmptyDiffTime, player3_Diff = EmptyDiffTime, player4_Diff = EmptyDiffTime, player5_Diff = EmptyDiffTime, player6_Diff = EmptyDiffTime;


        public RaceHUDViewModel()
        {
            RaceType defaultRaceType = new RaceType(
                RaceTypesEnum.FreePlay,
                "Free Play",
                "Players drive for the fun of it - no limit and no rules!",
                999,
                false,
                false,
                0,
                Symbol.Play);
            this.session = new RaceSession(defaultRaceType, new ObservableCollection<Driver>());
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

        public string Player1_GridNumber { get => this.Session.NumberOfDrivers >= 1 ? this.Session.Drivers[0].ControllerId.ToString() : String.Empty; set => Set(ref player1_GridNumber, value); }
        public string Player2_GridNumber { get => this.Session.NumberOfDrivers >= 2 ? this.Session.Drivers[1].ControllerId.ToString() : String.Empty; set => Set(ref player2_GridNumber, value); }
        public string Player3_GridNumber { get => this.Session.NumberOfDrivers >= 3 ? this.Session.Drivers[2].ControllerId.ToString() : String.Empty; set => Set(ref player3_GridNumber, value); }
        public string Player4_GridNumber { get => this.Session.NumberOfDrivers >= 4 ? this.Session.Drivers[3].ControllerId.ToString() : String.Empty; set => Set(ref player4_GridNumber, value); }
        public string Player5_GridNumber { get => this.Session.NumberOfDrivers >= 5 ? this.Session.Drivers[4].ControllerId.ToString() : String.Empty; set => Set(ref player5_GridNumber, value); }
        public string Player6_GridNumber { get => this.Session.NumberOfDrivers >= 6 ? this.Session.Drivers[5].ControllerId.ToString() : String.Empty; set => Set(ref player6_GridNumber, value); }
        public string Player1_Avatar => this.Session.NumberOfDrivers >= 1 ? this.Session.Drivers[0].ImageName : Driver.DefaultImageName;
        public string Player2_Avatar => this.Session.NumberOfDrivers >= 2 ? this.Session.Drivers[1].ImageName : Driver.DefaultImageName;
        public string Player3_Avatar => this.Session.NumberOfDrivers >= 3 ? this.Session.Drivers[2].ImageName : Driver.DefaultImageName;
        public string Player4_Avatar => this.Session.NumberOfDrivers >= 4 ? this.Session.Drivers[3].ImageName : Driver.DefaultImageName;
        public string Player5_Avatar => this.Session.NumberOfDrivers >= 5 ? this.Session.Drivers[4].ImageName : Driver.DefaultImageName;
        public string Player6_Avatar => this.Session.NumberOfDrivers >= 6 ? this.Session.Drivers[5].ImageName : Driver.DefaultImageName;
        public string Player1_Name => this.Session.NumberOfDrivers >= 1 ? this.Session.Drivers[0].UserName : String.Empty;
        public string Player2_Name => this.Session.NumberOfDrivers >= 2 ? this.Session.Drivers[1].UserName : String.Empty;
        public string Player3_Name => this.Session.NumberOfDrivers >= 3 ? this.Session.Drivers[2].UserName : String.Empty;
        public string Player4_Name => this.Session.NumberOfDrivers >= 4 ? this.Session.Drivers[3].UserName : String.Empty;
        public string Player5_Name => this.Session.NumberOfDrivers >= 5 ? this.Session.Drivers[4].UserName : String.Empty;
        public string Player6_Name => this.Session.NumberOfDrivers >= 6 ? this.Session.Drivers[5].UserName : String.Empty;

        public string BestLapHeaderText => "Best Lap";
        public string LastLapHeaderText => "Last Lap";
        public string DiffHeaderText => "Difference";
        
        public string Player1_BestLap { get => this.player1_BestLap; set => Set(ref player1_BestLap, value);}
        public string Player2_BestLap { get => this.player2_BestLap; set => Set(ref player2_BestLap, value); }
        public string Player3_BestLap { get => this.player3_BestLap; set => Set(ref player3_BestLap, value); }
        public string Player4_BestLap { get => this.player4_BestLap; set => Set(ref player4_BestLap, value); }
        public string Player5_BestLap { get => this.player5_BestLap; set => Set(ref player5_BestLap, value); }
        public string Player6_BestLap { get => this.player6_BestLap; set => Set(ref player6_BestLap, value); }
        public string Player1_LastLap { get => this.player1_LastLap; set => Set(ref player1_LastLap, value); }
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

        /// <summary>
        /// Event handler for the start button, starts a race timer and starts the Powerbase communications.
        /// </summary>
        internal void RaceButtonClicked()
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString()}: StartButton clicked.");

            if (this.Session.Started)
            {
                this.checkForRaceFinishCancellationTokenSource.Cancel();
                this.Session.ResetRace();
                SimpleIoc.Default.GetInstance<Powerbase>().UpdateRaceSession(this.Session);
                this.ResetDisplay();
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
        /// Resets fields to starting values.
        /// </summary>
        private void ResetDisplay()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                this.raceTimeDisplayTimer.Stop();
                this.RaceButtonBrush = this.greenBrush;
                this.RaceTimeDisplay = "00:00:00.0";
                this.RaceButtonText = "START";
                this.UpdateRemainingDisplay();
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
            });
        }

        /// <summary>
        /// Resets the session and display then navigates back to GridConfirmationPage if controllers are misconfigured.
        /// </summary>
        internal void GoBackToGridConfirmation()
        {
            this.Session.ResetRace();
            this.ResetDisplay();

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(GridConfirmationViewModel).FullName);
            });
        }

        /// <summary>
        /// Event handler for the quit race button,navigates to the Main (home) screen which stops the powerbase.
        /// </summary>
        internal void QuitButtonClicked()
        {
            this.checkForRaceFinishCancellationTokenSource.Cancel();
            this.Session.QuitRace();
            this.ResetDisplay();

            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
        }


        /// <summary>
        /// Event handler for displaying the race time duration from DispatcherTimer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountdownDisplayTimer_Tick(object sender, object e)
        {
            if (this.countdown == 0)
            {
                this.countdownDisplayTimer.Stop();
                this.RaceButtonBrush = this.redBrush;
                this.RaceButtonText = "RESET";
                this.Session.StartRace();
                SimpleIoc.Default.GetInstance<Powerbase>().ResetGameTimer();

                this.checkForRaceFinishCancellationTokenSource = new CancellationTokenSource();
                CancellationToken token = this.checkForRaceFinishCancellationTokenSource.Token;
                Task.Factory.StartNew(() => this.CheckForRaceFinish(token));

                // Setup race display timer
                this.raceTimeDisplayTimer = new DispatcherTimer();
                this.raceTimeDisplayTimer.Tick += RaceTimeDisplayTimer_Tick;
                this.raceTimeDisplayTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                this.raceTimeDisplayTimer.Start();
            }
            else
            {
                this.countdown -= 1;
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

        /// <summary>
        /// Asynchronously checks for the race session to have finished, either at end of race
        /// or after controllers are misconfigured.
        /// </summary>
        /// <param name="token">Cancellation token to use.</param>
        /// <returns>Task</returns>
        private async Task CheckForRaceFinish(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (this.Session.Finished)
                {
                    this.checkForRaceFinishCancellationTokenSource.Cancel();

                    if (this.Session.Started)
                    {
                        // race finished correctly
                        await this.RaceFinishedAsync();
                    }
                    else
                    {
                        // Controllers are misconfigured, stop and return to GridConfirmation
                        this.GoBackToGridConfirmation();
                    }

                    SimpleIoc.Default.GetInstance<Powerbase>().StopListening();
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// Race finished naturally, stop display and save session Id, update instance and navigate to Results.
        /// </summary>
        public async Task RaceFinishedAsync()
        {
            if (this.raceTimeDisplayTimer != null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    this.raceTimeDisplayTimer.Stop();
                });
            }

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                this.ResetDisplay();
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(RaceResultsViewModel).FullName, this.Session);
            });

            try
            {
                RaceSessionDTO sessionDTO = Mapper.Map<RaceSessionDTO>(this.Session);
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(AppManager.ServerHostURL);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
                    string endpoint = @"/api/RaceSessions";

                    HttpResponseMessage response = httpClient.PostAsJsonAsync(endpoint, sessionDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        sessionDTO = JsonConvert.DeserializeObject<RaceSessionDTO>(jsonResponse);
                        if (sessionDTO != null)
                        {
                            foreach (KeyValuePair<int, DriverResult> driver in this.Session.DriverResults)
                            {
                                endpoint = @"/api/DriverResults";
                                driver.Value.RaceSessionId = sessionDTO.Id;
                                DriverResultDTO driverResultDTO = Mapper.Map<DriverResultDTO>(driver.Value);
                                response = httpClient.PostAsJsonAsync(endpoint, driverResultDTO).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    jsonResponse = await response.Content.ReadAsStringAsync();
                                    driverResultDTO = JsonConvert.DeserializeObject<DriverResultDTO>(jsonResponse);
                                    if (driverResultDTO != null)
                                    {
                                        // Delete driver config
                                        endpoint = $@"/api/Drivers/{driverResultDTO.DriverId}";
                                        response = await httpClient.DeleteAsync(endpoint);

                                        // post laptimes
                                        endpoint = @"/api/LapTimes";
                                        List<LapTimeDTO> lapTimeDTOs = new List<LapTimeDTO>();
                                        int lap = 1;
                                        foreach (TimeSpan lapTime in driver.Value.LapTimes)
                                        {
                                            lapTimeDTOs.Add(new LapTimeDTO()
                                            {
                                                DriverResultId = driverResultDTO.Id,
                                                LapNumber = lap++,
                                                Time = lapTime,
                                            });
                                        }

                                        response = httpClient.PostAsJsonAsync(endpoint, lapTimeDTOs).Result;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Called by race session to update a given players lap times when crossed the finish line.
        /// </summary>
        /// <param name="carId">The player to refresh (0 index).</param>
        internal void UpdateLapTimes(int carId)
        {
            TimeSpan diff = this.Session.DriverResults[carId].PreviousLapTime - this.Session.DriverResults[carId].BestLapTime;
            string sign = diff > this.zeroTimeSpan ? "+" : "-";
            string diffString = sign + diff.ToString(@"ss\.fff");
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                // Dispatch back to the main thread
                switch (this.Session.DriverResults[carId].ControllerId)
                {
                    case 1:
                        Player1_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player1_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player1_Diff = diffString;
                        break;
                    case 2:
                        Player2_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player2_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player2_Diff = $"{sign}{diff.ToString(@"ss\.fff")}";
                        break;
                    case 3:
                        Player3_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player3_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player3_Diff = $"{sign}{diff.ToString(@"ss\.fff")}";
                        break;
                    case 4:
                        Player4_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player4_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player4_Diff = $"{sign}{diff.ToString(@"ss\.fff")}";
                        break;
                    case 5:
                        Player5_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player5_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player5_Diff = $"{sign}{diff.ToString(@"ss\.fff")}";
                        break;
                    case 6:
                        Player6_BestLap = this.Session.DriverResults[carId].BestLapTime.ToString("m':'ss'.'fff");
                        Player6_LastLap = this.Session.DriverResults[carId].PreviousLapTime.ToString("m':'ss'.'fff");
                        Player6_Diff = $"{sign}{diff.ToString(@"ss\.fff")}";
                        break;
                    default:
                        break;
                }
            });
        }

        public override void OnNavigatedFrom()
        {
        }

        public async Task LoadDataAsync()
        {
            if (this.session != null)
            {
                if (!SimpleIoc.Default.GetInstance<Powerbase>().IsPowerbaseConnected)
                {
                    SimpleIoc.Default.GetInstance<Powerbase>().Listen(this.Session);
                }
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode)
        {
            if (parameter == null)
            {
                SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
            }
            else
            {
                this.session = parameter as RaceSession;

                switch (this.Session.NumberOfDrivers)
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

                this.RaceButtonBrush = this.greenBrush;
                this.RaceTimeDisplay = "00:00:00.0";
                this.RaceButtonText = "START";
                this.UpdateRemainingDisplay();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates the remaining laps/time display.
        /// </summary>
        private void UpdateRemainingDisplay()
        {
            if (this.Session.RaceType.LapsNotDuration)
            {
                this.RemainingDisplay = $"{this.session.LapsRemaining} / {this.session.RaceType.RaceLimitValue} Laps";
            }
            else
            {
                this.RemainingDisplay = this.Session.RaceType.RaceLength.ToString("hh':'mm':'ss'.'f");
            }
        }

        public override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!this.Session.Finished)
            {
                this.Session.Finished = true;
            }

//            SimpleIoc.Default.GetInstance<Powerbase>().StopListening();
        }
    }
}
