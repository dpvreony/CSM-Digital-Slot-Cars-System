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
                new Driver(1, "Tyler", "/Assets/UserImages/1.png", 1, 1)
                ,new Driver(2,"Armshaw", "/Assets/UserImages/2.png", 2, 2)
                ,new Driver(3,"Bowes", "/Assets/UserImages/3.png", 3, 3)
                ,new Driver(4,"Tyler", "/Assets/UserImages/4.png", 4, 4)
                ,new Driver(5,"Atkins", "/Assets/UserImages/5.png", 5, 5)
                ,new Driver(6,"Botten", "/Assets/UserImages/6.png", 6, 6)
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
        public static Driver GetUser(int userId)
        {
            if (users == null)
            {
                LoggedInUsers();
            }
            return users.SingleOrDefault(u => u.Id == userId);
        }
    }
}
