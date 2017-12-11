using System;

using SlotCarsGo_GUI.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
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
    }
}
