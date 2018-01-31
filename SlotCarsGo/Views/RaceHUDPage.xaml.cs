using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Ioc;

namespace SlotCarsGo.Views
{
    public sealed partial class RaceHUDPage : Page
    {
        private RaceHUDViewModel ViewModel
        {
            get { return DataContext as RaceHUDViewModel; }
        }

        public RaceHUDPage()
        {
            InitializeComponent();
        }

        private void StartRaceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StartButtonClicked();
            Button button = sender as Button;
            button.IsEnabled = false;
        }

        private void StopRaceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StopButtonClicked();
            Button button = sender as Button;
            button.IsEnabled = false;
        }
    }
}
