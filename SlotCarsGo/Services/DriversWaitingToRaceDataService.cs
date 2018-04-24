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

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Driver>> GetDriversWaitingToRaceAsync()
        {
            await Task.CompletedTask;

            return await LoggedInUsers();
        }
    }
}
