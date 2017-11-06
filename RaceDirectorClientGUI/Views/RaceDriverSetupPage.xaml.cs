using System;

using RaceDirectorClientGUI.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RaceDirectorClientGUI.Views
{
    public sealed partial class RaceDriverSetupPage : Page
    {
        private RaceDriverSetupViewModel ViewModel
        {
            get { return DataContext as RaceDriverSetupViewModel; }
        }

        public RaceDriverSetupPage()
        {
            InitializeComponent();
            Loaded += RaceDriverSetupPage_Loaded;
        }

        private async void RaceDriverSetupPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
