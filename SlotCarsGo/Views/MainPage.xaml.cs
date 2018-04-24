using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Models.Comms;
using SlotCarsGo.Services;
using Windows.UI.Xaml;

namespace SlotCarsGo.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(800, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void RaceTypeSelectButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RaceTypeSelectClicked();
        }

        private void GarageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.GarageButtonClicked();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(SettingsViewModel).FullName);
        }
    }
}
