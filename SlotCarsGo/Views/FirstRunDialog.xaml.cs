using SlotCarsGo.Models.Manager;
using SlotCarsGo.Services;
using System;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public bool TrackNameEntered { get => this.RegisterTrackName_TextBox.Text != String.Empty; }

        public FirstRunDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string trackName = RegisterTrackName_TextBox.Text;
            if (trackName != String.Empty)
            {
                AppManager.RegisterTrackOnStartup(trackName);
                this.Hide();
            }
            else
            {
                AppManager.MakeToast("Track name cannot be empty!"); //TODO: fix (use sample)
            }
        }
    }
}
