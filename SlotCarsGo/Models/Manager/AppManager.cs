using AutoMapper;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;


namespace SlotCarsGo.Models.Manager
{
    public class AppManager
    {
        public static Track Track { get; set; }
        public static readonly string ServerHostURL;
        public static StorageFolder TemporaryFolder { get; set; }

        private static ToastNotificationsService toastService;
        private static ApplicationDataContainer localSettings;
        private static StorageFolder localFolder;

        /// <summary>
        /// Static constructor for AppManager class.
        /// </summary>
        static AppManager()
        {
            ServerHostURL = @"https://slotcarsgo.timtyler.co.uk";
            AppManager.localSettings = ApplicationData.Current.LocalSettings;
            AppManager.localFolder = ApplicationData.Current.LocalFolder;
            AppManager.TemporaryFolder = ApplicationData.Current.TemporaryFolder;
            AppManager.toastService = new ToastNotificationsService();
            AppManager.Track = new Track()
            {
                Name = (string)localSettings.Values["TrackName"],
                Id = (string)localSettings.Values["TrackId"],
                Secret = (string)localSettings.Values["Secret"]
                //                Length = (float)trackCompositeValue["Length"],
                //                MacAddress = (string)trackCompositeValue["MacAddress"],
            };

            ThemeSelectorService.Theme = Windows.UI.Xaml.ElementTheme.Dark;

            Mapper.Initialize(cfg => {
                cfg.CreateMap<RaceSession, RaceSessionDTO>()
                    .ForMember(dest => dest.RaceLimitValue, opt => opt.MapFrom(src => src.RaceType.RaceLimitValue))
                    .ForMember(dest => dest.RaceTypeId, opt => opt.MapFrom(src => src.RaceType.Name))
                    .ForMember(dest => dest.RaceLength, opt => opt.MapFrom(src => src.RaceType.RaceLength))
                    .ForMember(dest => dest.LapsNotDuration, opt => opt.MapFrom(src => src.RaceType.LapsNotDuration))
                    .ForMember(dest => dest.CrashPenalty, opt => opt.MapFrom(src => src.RaceType.CrashPenalty));
                cfg.CreateMap<DriverResult, DriverResultDTO>()
                    .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.Driver.UserId))
                    .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Car.Id))
                    .ForMember(dest => dest.Laps, opt => opt.MapFrom(src => src.LapTimes.Count));
                cfg.CreateMap<TrackDTO, Track>();
                cfg.CreateMap<CarDTO, Car>();
                cfg.CreateMap<DriverDTO, Driver>()
                    .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.SelectedCar))
                    .ReverseMap()
                        .ForMember(src => src.SelectedCar, opt => opt.MapFrom(dest => dest.SelectedCar));
                cfg.CreateMap<RaceTypeDTO, RaceType>();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public AppManager()
        {


        }


        /// <summary>
        /// Registers the track name on the server and saves the name and Id to local settings.
        /// </summary>
        /// <param name="trackName"></param>
        public async static Task<string> RegisterTrackOnStartup(string trackName, string email)
        {
            TrackDTO trackDTO = new TrackDTO() { Name = trackName, OwnerEmail = email, Length = 0f };
            string returnSecret = string.Empty;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ServerHostURL);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
                    string endpoint = @"/api/tracks";

                    HttpResponseMessage response = httpClient.PostAsJsonAsync(endpoint, trackDTO).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        trackDTO = JsonConvert.DeserializeObject<TrackDTO>(jsonResponse);
                        if (trackDTO != null)
                        {
                            AppManager.Track = new Track()
                            {
                                Name = trackDTO.Name,
                                Id = trackDTO.Id,
                                Secret = trackDTO.Secret
                                //                Length = (float)trackCompositeValue["Length"],
                                //                MacAddress = (string)trackCompositeValue["MacAddress"],
                            };

                            localSettings.Values["TrackName"] = trackDTO.Name;
                            localSettings.Values["TrackId"] = trackDTO.Id;
                            localSettings.Values["Secret"] = trackDTO.Secret;
                            // TODO: trackCompositeValue["Length"] = length;
                            // TODO: trackCompositeValue["MacAddress"] = macAddress; https://stackoverflow.com/questions/34097870/c-sharp-get-mac-address-in-universal-apps

                            returnSecret = trackDTO.Secret;
                        }
                    }

                }
            }
            catch (Exception)
            {
            }

            return returnSecret;
        }

        /// <summary>
        /// Registers the track name on the server and saves the name and Id to local settings.
        /// </summary>
        /// <param name="trackName"></param>
        public async static Task<string> RegisterExistingTrackOnStartup(string secret)
        {
            TrackDTO trackDTO = new TrackDTO() { Secret = secret, Length = 0f };
            string returnName = string.Empty;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
                var uri = new Uri($@"{AppManager.ServerHostURL}/api/Tracks/{secret}");

                try
                {
                    var response = await Task.Run(async () => await httpClient.GetAsync(uri));
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        trackDTO = await Json.ToObjectAsync<TrackDTO>(content);
                        if (trackDTO != null)
                        {
                            AppManager.Track = new Track()
                            {
                                Name = trackDTO.Name,
                                Id = trackDTO.Id,
                                Secret = trackDTO.Secret
                                //                Length = (float)trackCompositeValue["Length"],
                                //                MacAddress = (string)trackCompositeValue["MacAddress"],
                            };

                            localSettings.Values["TrackName"] = trackDTO.Name;
                            localSettings.Values["TrackId"] = trackDTO.Id;
                            localSettings.Values["Secret"] = trackDTO.Secret;
                            // TODO: trackCompositeValue["Length"] = length;
                            // TODO: trackCompositeValue["MacAddress"] = macAddress; https://stackoverflow.com/questions/34097870/c-sharp-get-mac-address-in-universal-apps

                            returnName = trackDTO.Name;
                        }

                    }
                }
                catch (Exception e)
                {

                }
            }

            return returnName;
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

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            AppManager.toastService.ShowToastNotification(toast);
        }
    }
}
