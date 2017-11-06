using System;

using RaceDirectorClientGUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace RaceDirectorClientGUI.Views
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
