using System;

using RaceDirectorClientGUI.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using RaceDirectorClientGUI.Models.Comms;
using RaceDirectorClientGUI.Models;
using System.Collections.Generic;
using RaceDirectorClientGUI.Models.Racing;

namespace RaceDirectorClientGUI
{
    public sealed partial class App : Application
    {
        private Powerbase pb;
        private Lazy<ActivationService> _activationService;

        internal Powerbase Powerbase { get => this.pb; }

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);


            /// ALL THIS MOVES TO A MANAGER CLASS
            Console.WriteLine($"{DateTime.Now.ToString()}: Starting 'CSM Digital Slot Cars System'");
            List<Player> players = new List<Player> { new Player() };
            RaceTypeBase raceType = new FreePlayRace(5, new TimeSpan(0, 2, 0), true);
            RaceSession raceSession = new RaceSession(1, raceType, players, false);
            this.pb = new Powerbase();
            raceSession.RaceStart(this.Powerbase);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.MainViewModel), new Views.ShellPage());
        }
    }
}
