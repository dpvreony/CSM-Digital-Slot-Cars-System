using SlotCarsGo.Models.Racing;
using SlotCarsGo.Services;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SlotCarsGo.Models.Manager
{
    public class Driver
    {
        int id;
        string username;
        string imageName;
        int controllerId;
        Car selectedCar;

        public Driver(int id, string username, string imageName, int controllerId, int carId)
        {
            this.Id = id;
            this.UserName = username;
            this.ImageName = imageName;
            this.ControllerId = controllerId;
            // TODO: remove constructor from client version as server will return objects.
            this.SelectedCar = CarsInGarageDataService.GetCar(carId);
        }

        public int Id { get => id; set => id = value; }
        public string UserName { get => username; set => username = value; }
        public string ImageName { get => imageName; set => imageName = value; }
        public int ControllerId { get => controllerId; set => controllerId = value; }
        internal Car SelectedCar { get => selectedCar; set => selectedCar = value; }

        private static Driver defaultDriver = new Driver(0, String.Empty, "0.png", 1, 1);
        public static Driver DefaultDriver { get => defaultDriver; }
    }
}
