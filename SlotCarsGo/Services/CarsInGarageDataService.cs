using AutoMapper;
using SlotCarsGo.Helpers;
using SlotCarsGo.Models.Manager;
using SlotCarsGo.Models.Racing;
using SlotCarsGo_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Services
{
    public static class CarsInGarageDataService
    {
        private static ObservableCollection<Car> garage;

        private async static Task GetCars()
        {
            garage = new ObservableCollection<Car>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
                var uri = new Uri($@"{AppManager.ServerHostURL}/api/Cars/{AppManager.Track.Id}");

                try
                {
                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var json = await Json.ToObjectAsync<IEnumerable<CarDTO>>(content);
                        foreach (CarDTO carDTO in json)
                        {
                            StorageFile savedCarImage = await AppManager.TemporaryFolder.CreateFileAsync(carDTO.ImageName, CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteBytesAsync(savedCarImage, carDTO.CarImageBytes);
                            carDTO.ImageName = savedCarImage.Path;
                            carDTO.CarImageBytes = null;

                            garage.Add(Mapper.Map<Car>(carDTO));
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Car>> GetCarsInGarageAsync()
        {
            await Task.CompletedTask;

            if (garage == null)
            {
                await GetCars();
            }

            return garage;
        }

        /// <summary>
        /// Returns the car in collection with matching carId.
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public static async Task<Car> GetCar(string carId)
        {
            if (garage == null)
            {
                await GetCars();
            }

            return garage.SingleOrDefault(c => c.Id == carId);
        }
    }
}
