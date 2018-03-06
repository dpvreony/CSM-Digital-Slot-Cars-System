using SlotCarsGo.Models.Manager;
using SlotCarsGo.Models.Racing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SlotCarsGo.Services
{
    public static class LoggedInUsersDataService
    {
        private static ObservableCollection<Driver> users;

        private static IEnumerable<Driver> LoggedInUsers()
        {
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
            
            users = new ObservableCollection<Driver>(data.OrderBy(u => u.ControllerId));

            return users;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Driver>> GetLoggedInUsersAsync()
        {
            await Task.CompletedTask;

            return LoggedInUsers();
        }

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
    }
}
