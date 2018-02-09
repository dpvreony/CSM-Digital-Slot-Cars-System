using SlotCarsGo.Models.Racing;
using SlotCarsGo.Services;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SlotCarsGo.Models.Manager
{
    public class User
    {
        int id;
        string nickname;
        string firstname;
        string lastname;
        string avatarSource;
        int controllerId;
        Car selectedCar;

        public User(int id, string nickname, string firstname, string lastname, string avatarPath, int carId)
        {
            this.id = id;
            this.nickname = nickname;
            this.firstname = firstname;
            this.lastname = lastname;
            this.avatarSource = avatarPath;
            this.selectedCar = CarsInGarageDataService.GetCar(carId);
        }

        public int Id { get => id; set => id = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string AvatarSource { get => avatarSource; set => avatarSource = value; }
        public int ControllerId { get => controllerId; set => controllerId = value; }
        internal Car SelectedCar { get => selectedCar; set => selectedCar = value; }

        private static User defaultUser = new User(0, String.Empty, String.Empty, String.Empty, "/Assets/UserImages/0.png", new Car(0, String.Empty));
        public static User DefaultUser { get => defaultUser; }

    }
}
