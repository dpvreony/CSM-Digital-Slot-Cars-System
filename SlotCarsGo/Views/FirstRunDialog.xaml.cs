using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        private bool Registered = false;

        public FirstRunDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Registered)
            {
                this.Hide();
            }
            else
            {
                string trackName = RegisterTrackName_TextBox.Text;
                if (trackName != String.Empty)
                {
                    string secret = AppManager.RegisterTrackOnStartup(trackName).Result;
                    if (string.IsNullOrEmpty(secret))
                    {
                        AppManager.MakeToast("Track not registered, check internet connection."); //TODO: fix (use sample)
                    }
                    else
                    {
                        this.PostRegistrationPanel.Visibility = Visibility.Visible;
                        this.RegisterMessageText.Visibility = Visibility.Visible;
                        this.ConfirmButton.Content = "OK";
                        this.TrackSecret_TextBox.Text = secret;
                        this.Registered = true;
                    }
                }
                else
                {
                    AppManager.MakeToast("Track name cannot be empty!"); //TODO: fix (use sample)
                }
            }
        }
    }
}
