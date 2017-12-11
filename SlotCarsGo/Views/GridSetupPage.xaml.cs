using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Views
{
    public sealed partial class GridSetupPage : Page
    {
        private GridSetupViewModel ViewModel
        {
            get { return DataContext as GridSetupViewModel; }
        }

        public GridSetupPage()
        {
            InitializeComponent();
            Loaded += GridSetupPage_Loaded;
        }

        private async void GridSetupPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
