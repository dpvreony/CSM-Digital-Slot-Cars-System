using System;

using SlotCarsGo_GUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
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
    }
}
