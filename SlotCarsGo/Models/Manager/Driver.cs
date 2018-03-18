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

        public Driver()
        {
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public int ControllerId { get; set; }
        public Car SelectedCar { get; set; }


        internal static string DefaultUserName = "No name set";
        internal static string DefaultImageName = "0.png";

    }
}
