using System;

using SlotCarsGo_GUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
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
        }
    }
}
