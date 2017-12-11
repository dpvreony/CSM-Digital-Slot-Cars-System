using RaceDirectorClientGUI.Models.Manager;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo_GUI.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string trackName = RegisterTrackName_TextBox.Text;
            AppManager.RegisterTrackOnStartup(trackName);
        }
    }
}
