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
        string nickname;
        string firstname;
        string lastname;
        string avatarSource;
        int controllerId;
        Car selectedCar;

        public Driver(int id, string nickname, string firstname, string lastname, string avatarPath, int controllerId, int carId)
        {
            this.Id = id;
            this.Nickname = nickname;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.AvatarSource = avatarPath;
            this.ControllerId = controllerId;
            this.SelectedCar = CarsInGarageDataService.GetCar(carId);
        }

        public int Id { get => id; set => id = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string AvatarSource { get => avatarSource; set => avatarSource = value; }
        public int ControllerId { get => controllerId; set => controllerId = value; }
        internal Car SelectedCar { get => selectedCar; set => selectedCar = value; }

        private static Driver defaultDriver = new Driver(0, String.Empty, String.Empty, String.Empty, "/Assets/UserImages/0.png", 1, 1);
        public static Driver DefaultDriver { get => defaultDriver; }

    }
}
