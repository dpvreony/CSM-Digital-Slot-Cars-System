using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml;

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
            Loaded += RaceHUDPage_Loaded;
        }

        private async void RaceHUDPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        private void RaceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RaceButtonClicked();
        }

        private void QuitRaceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //            ViewModel.QuitButtonClicked();
            ViewModel.RaceFinishedAsync();
        }
    }
}
