using AutoMapper;
using GalaSoft.MvvmLight.Ioc;
using SlotCarsGo.Helpers;
using SlotCarsGo.Models.Comms;
using SlotCarsGo.Models.Racing;
using SlotCarsGo.Services;
using SlotCarsGo.ViewModels;
using SlotCarsGo.Views;
using SlotCarsGo_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace SlotCarsGo.Models.Manager
{
    public class AppManager
    {
        private static ToastNotificationsService toastService;
        private static ApplicationDataContainer localSettings;
        private static StorageFolder localFolder;
        private static Track track = new Track("Tim's Raceway", String.Empty, 3.6f, "00:34:A9:DE:29"); // TODO: replace with real data

        /// <summary>
        /// Static constructor for AppManager class.
        /// </summary>
        static AppManager()
        {
            AppManager.localSettings = ApplicationData.Current.LocalSettings;
            AppManager.localFolder = ApplicationData.Current.LocalFolder;
            AppManager.toastService = new ToastNotificationsService();
            AppManager.localSettings.Values["Track"] = null;
            var trackCompositeValue = (ApplicationDataCompositeValue)localSettings.Values["Track"];
            if (trackCompositeValue != null)
            {
                AppManager.track = new Track(
                    (string)trackCompositeValue["TrackName"],
                    (string)trackCompositeValue["TrackId"],
                    (float)trackCompositeValue["Length"],
                    (string)trackCompositeValue["MacAddress"]);
            }
            ThemeSelectorService.Theme = Windows.UI.Xaml.ElementTheme.Dark;

            Mapper.Initialize(cfg => {
                cfg.CreateMap<RaceSessionDTO, RaceSession>();
                cfg.CreateMap<DriverResultDTO, DriverResult>();
                cfg.CreateMap<TrackDTO, Track>();
                cfg.CreateMap<CarDTO, Car>();
                cfg.CreateMap<RaceTypeDTO, RaceType>();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public AppManager()
        {


        }

        internal static Track Track { get => AppManager.track; }

        /// <summary>
        /// Registers the track name on the server and saves the name and Id to local settings.
        /// </summary>
        /// <param name="trackName"></param>
        public async static Task<string> RegisterTrackOnStartup(string trackName)
        {
            string trackId = String.Empty; // TODO: contact server for trackId
            float length = 0.0f;
            string macAddress = String.Empty; // TODO: get Mac https://stackoverflow.com/questions/34097870/c-sharp-get-mac-address-in-universal-apps
            AppManager.track = new Track(trackName, trackId, length, macAddress);
            string secret = "SECRET" // get from returned track. TODO: show in settings

            ApplicationDataCompositeValue trackCompositeValue = new ApplicationDataCompositeValue();
            trackCompositeValue["TrackName"] = trackName;
            trackCompositeValue["TrackId"] = trackId;
            trackCompositeValue["Length"] = length;
            trackCompositeValue["Secret"] = secret;
            trackCompositeValue["MacAddress"] = macAddress;
            await SettingsStorageExtensions.SaveAsync(localSettings, "Track", trackCompositeValue);

            return secret;
        }

        /// <summary>
        /// Generates a single line toast notification similar to Android.
        /// Ref: https://stackoverflow.com/questions/37541923/how-to-create-informative-toast-notification-in-uwp-app
        /// </summary>
        /// <param name="title">The toast title.</param>
        /// <param name="stringContent">The toast content.</param>
        public static void MakeToast(string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(stringContent));
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

//            ToastNotification toast = new ToastNotification(toastXml);
//            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            // AppManager.toastService.ShowToastNotification(toast);
        }
    }
}
