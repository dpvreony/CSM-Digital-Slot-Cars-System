using SlotCarsGo_GUI.Models.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RaceDirectorClientGUI.Models.Manager
{
    class AppManager
    {
        static ApplicationDataContainer localSettings;
        static StorageFolder localFolder;

        Powerbase powerbase;
        Track track;

        /// <summary>
        /// 
        /// </summary>
        public AppManager()
        {
            this.powerbase = new Powerbase();
            AppManager.localSettings = ApplicationData.Current.LocalSettings;
            AppManager.localFolder = ApplicationData.Current.LocalFolder;

            string trackName = (string)AppManager.localSettings.Values["TrackName"];
            int trackId = (int)AppManager.localSettings.Values["TrackId"];

            if (trackName != null && trackId != 0)
            {
                this.Track = new Track(trackName, trackId);
            }

        }

        public static ApplicationDataContainer LocalSettings { get => AppManager.localSettings; }
        public static StorageFolder LocalFolder { get => AppManager.localFolder; }
        internal Track Track { get => track; set => track = value; }

        public static void RegisterTrackOnStartup(string trackName)
        {
            AppManager.LocalSettings.Values["TrackName"] = trackName;
            // TODO: contact server for trackId
            AppManager.LocalSettings.Values["TrackId"] = 1; //HACK
        }
    }
}
