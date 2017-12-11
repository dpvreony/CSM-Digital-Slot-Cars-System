﻿using System;

using SlotCarsGo.ViewModels;

using Windows.UI.Xaml.Controls;

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
        }

        private void StartButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.ConfirmGridAndProceed();
        }

        private void RefreshButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RefreshLoggedInUsers();
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
