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
        private bool TrackFound = false;

        public FirstRunDialog()
        {
            InitializeComponent();
        }

        private void NewTrackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Registered)
            {
                this.Hide();
            }
            else
            {
                string trackName = NewTrackName_TextBox.Text;
                string email = NewTrackEmail_TextBox.Text;

                if (trackName != String.Empty)
                {
                    string secret = AppManager.RegisterTrackOnStartup(trackName, email).Result;
                    if (string.IsNullOrEmpty(secret))
                    {
                        this.NewBannerText.Text = "Track not registered, check internet connection.";
                        AppManager.MakeToast("Track not registered, check internet connection."); //TODO: fix (use sample)
                    }
                    else
                    {
                        this.NewBannerText.Text = $"Login to {AppManager.ServerHostURL} to join this track.";
                        this.NewTrackConfirmButton.Content = "OK";
                        this.NewTrackSecret_TextBox.Text = secret;
                        this.Registered = true;
                    }
                }
                else
                {
                    this.NewBannerText.Text = "Track name cannot be empty!";
                    AppManager.MakeToast("Track name cannot be empty!");
                }
            }
        }

        private async void ExistingTrackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (TrackFound)
            {
                this.Hide();
            }
            else
            {
                string secret = ExistingTrackSecret_TextBox.Text;
                if (secret != String.Empty)
                {
                    string name = await AppManager.RegisterExistingTrackOnStartup(secret);
                    if (string.IsNullOrEmpty(name))
                    {
                        this.ExistingBannerText.Text = $"Track not found, check secret.";
                        AppManager.MakeToast("Track not found, check the secret.");
                    }
                    else
                    {
                        this.ExistingBannerText.Text = $"Track '{name}' found.";
                        this.ExistingConfirmButton.Content = "OK";
                        this.TrackFound = true;
                    }
                }
                else
                {
                    this.NewBannerText.Text = "Track secret cannot be empty!";
                    AppManager.MakeToast("Track name cannot be empty!");
                }
            }
        }
    }
}
