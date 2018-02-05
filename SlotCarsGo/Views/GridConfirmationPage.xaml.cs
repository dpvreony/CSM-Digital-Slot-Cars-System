using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SlotCarsGo.Views
{
    public sealed partial class GridConfirmationPage : Page
    {
        SolidColorBrush greenBrush = new SolidColorBrush(Windows.UI.Colors.LimeGreen);
        SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.OrangeRed);

        private GridConfirmationViewModel ViewModel
        {
            get { return DataContext as GridConfirmationViewModel; }
        }

        public GridConfirmationPage()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ConfirmGridAndProceed();
        }

        private void RefreshButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RefreshLoggedInUsers();
            ConfirmUser1Button.Background = redBrush;
            ConfirmUser2Button.Background = redBrush;
            ConfirmUser3Button.Background = redBrush;
            ConfirmUser4Button.Background = redBrush;
            ConfirmUser5Button.Background = redBrush;
            ConfirmUser6Button.Background = redBrush;
        }

        private void ConfirmUserButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "ConfirmUser1Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(0);
                    break;
                case "ConfirmUser2Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(1);
                    break;
                case "ConfirmUser3Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(2);
                    break;
                case "ConfirmUser4Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(3);
                    break;
                case "ConfirmUser5Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(4);
                    break;
                case "ConfirmUser6Button":
                    button.Background = greenBrush;
                    ViewModel.RemoveLoggedInUser(5);
                    break;
                default:
                    break;
            }
        }

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
