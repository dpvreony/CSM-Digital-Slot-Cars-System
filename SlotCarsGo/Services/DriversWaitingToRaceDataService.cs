using AutoMapper;
using SlotCarsGo.Helpers;
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Models.Racing;
using SlotCarsGo_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SlotCarsGo.Services
{
    public static class DriversWaitingToRaceDataService
    {
        private async static Task<IEnumerable<Driver>> LoggedInUsers()
        {
            ObservableCollection<Driver> drivers = new ObservableCollection<Driver>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
                var uri = new Uri($@"{AppManager.ServerHostURL}/api/Drivers/{AppManager.Track.Id}");

                try
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var json = await Json.ToObjectAsync<IEnumerable<DriverDTO>>(content);
                        foreach (DriverDTO driverDTO in json)
                        {
                            StorageFile savedUserImage = await AppManager.TemporaryFolder.CreateFileAsync(driverDTO.ImageName, CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteBytesAsync(savedUserImage, driverDTO.UserImageBytes);
                            driverDTO.ImageName = savedUserImage.Path;
                            driverDTO.UserImageBytes = null;

                            StorageFile savedCarImage = await AppManager.TemporaryFolder.CreateFileAsync(driverDTO.SelectedCar.ImageName, CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteBytesAsync(savedCarImage, driverDTO.SelectedCar.CarImageBytes);
                            driverDTO.SelectedCar.ImageName = savedCarImage.Path;
                            driverDTO.SelectedCar.CarImageBytes = null;

                            drivers.Add(Mapper.Map<Driver>(driverDTO));
                        }
                    }
                }
                catch (Exception e)
                {

                }

                return drivers;
            }
        }

                        /*
                                                BitmapImage userImage, carImage;
                                                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                                                {
                                                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                                                    {
                                                        writer.WriteBytes(driverDTO.UserImageBytes);
                                                        writer.StoreAsync().GetResults();
                                                    }
                                                    userImage = new BitmapImage();
                                                    userImage.SetSource(ms);
                                                }


                                                /*
                                    m_fileToken = m_localSettings["scenario1FileToken"].ToString();
                                    StorageFile file = await m_futureAccess.GetFileAsync(m_fileToken);

                                    BitmapImage src = new BitmapImage();
                                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                                    {
                                        await src.SetSourceAsync(stream);
                                    }

                                    PreviewImage.Source = src;


                                                StorageFile savedImage
                                                 savedImage.

                                                //                        Image userImage = driver.UserImageBytes
                                                StorageFile savedImage = AppManager.TemporaryFolder.
                        //                        string imageName = "path in temp fodler";
                                               */


                /*
                            // TODO: Query logins from server
                            var data = new ObservableCollection<Driver>
                            {
                                new Driver("1", "Tyler", "1.png", 1, Car.DefaultCar)
                                ,new Driver("2","Armshaw", "2.png", 2, Car.DefaultCar)
                                ,new Driver("3","Bowes", "3.png", 3, Car.DefaultCar)
                                ,new Driver("4","Tyler", "4.png", 4, Car.DefaultCar)
                                ,new Driver("5","Atkins", "5.png", 5, Car.DefaultCar)
                                ,new Driver("6","Botten", "6.png", 6, Car.DefaultCar)
                            };
                */
                //            users = new ObservableCollection<Driver>(data.OrderBy(u => u.ControllerId));

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Driver>> GetDriversWaitingToRaceAsync()
        {
            await Task.CompletedTask;

            return await LoggedInUsers();
        }

/*
        /// <summary>
        /// Returns the user in collection with matching userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Driver GetUser(string userId)
        {
            if (users == null)
            {
                LoggedInUsers();
            }
            return users.SingleOrDefault(u => u.Id == userId);
        }
*/
    }
}
