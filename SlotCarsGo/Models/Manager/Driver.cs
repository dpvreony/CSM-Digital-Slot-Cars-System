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
        string id;
        string username;
        string imageName;
        int controllerId;
        Car selectedCar;

        public Driver(string id, string username, string imageName, int controllerId, Car car)
        {
            this.Id = id;
            this.UserName = username;
            this.ImageName = "/Assets/UserImages/" + imageName;
            this.ControllerId = controllerId;
            this.SelectedCar = car;
        }

        public string Id { get => id; set => id = value; }
        public string UserName { get => username; set => username = value; }
        public string ImageName { get => imageName; set => imageName = value; }
        public int ControllerId { get => controllerId; set => controllerId = value; }
        internal Car SelectedCar { get => selectedCar; set => selectedCar = value; }

        public static readonly Driver DefaultDriver = new Driver("id", "Default User", "0.png", 1, Car.DefaultCar);
    }
}
