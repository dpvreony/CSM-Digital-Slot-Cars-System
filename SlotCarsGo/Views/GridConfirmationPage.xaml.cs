using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SlotCarsGo.Views
{
    public sealed partial class GridConfirmationPage : Page
    {
        private GridConfirmationViewModel ViewModel
        {
            get { return DataContext as GridConfirmationViewModel; }
        }

        public GridConfirmationPage()
        {
            InitializeComponent();
            Loaded += GridConfirmationPage_Loaded;
        }

        private async void GridConfirmationPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        private void StartButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ConfirmGridAndProceed();
        }

        private void RefreshButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RefreshLoggedInUsers();
        }

        private void ConfirmUserButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "ConfirmUser1Button":
                    ViewModel.ConfirmUserOnGrid(0);
                    break;
                case "ConfirmUser2Button":
                    ViewModel.ConfirmUserOnGrid(1);
                    break;
                case "ConfirmUser3Button":
                    ViewModel.ConfirmUserOnGrid(2);
                    break;
                case "ConfirmUser4Button":
                    ViewModel.ConfirmUserOnGrid(3);
                    break;
                case "ConfirmUser5Button":
                    ViewModel.ConfirmUserOnGrid(4);
                    break;
                case "ConfirmUser6Button":
                    ViewModel.ConfirmUserOnGrid(5);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Remove button event handler to remove a player from the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveUserButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "RemoveUser1Button":
                    ViewModel.RemoveLoggedInUser(0);
                    break;
                case "RemoveUser2Button":
                    ViewModel.RemoveLoggedInUser(1);
                    break;
                case "RemoveUser3Button":
                    ViewModel.RemoveLoggedInUser(2);
                    break;
                case "RemoveUser4Button":
                    ViewModel.RemoveLoggedInUser(3);
                    break;
                case "RemoveUser5Button":
                    ViewModel.RemoveLoggedInUser(4);
                    break;
                case "RemoveUser6Button":
                    ViewModel.RemoveLoggedInUser(5);
                    break;
                default:
                    break;
            }
        }
    }
}
