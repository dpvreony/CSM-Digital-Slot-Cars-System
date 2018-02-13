using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Services;

namespace SlotCarsGo.Views
{
    public sealed partial class RaceResultsPage : Page
    {
        private RaceResultsViewModel ViewModel
        {
            get { return DataContext as RaceResultsViewModel; }
        }

        public RaceResultsPage()
        {
            InitializeComponent();
            Loaded += RaceResultsPage_Loaded;
        }

        private async void RaceResultsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<NavigationServiceEx>().Navigate(typeof(MainViewModel).FullName);
        }
    }
}
