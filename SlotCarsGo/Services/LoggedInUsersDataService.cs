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
        private static ObservableCollection<User> users;

        private static IEnumerable<User> LoggedInUsers()
        {
            // TODO: Query logins 
            var data = new ObservableCollection<User>
            {
                new User(1, "Tyler", "Tim", "Tyler", "/Assets/UserImages/1.png", 1)
                ,new User(2, "Cake", "Dave", "Armshaw", "/Assets/UserImages/2.png", 2)
                ,new User(3, "Bowes", "Adam", "Bowes", "/Assets/UserImages/3.png", 3)
                ,new User(4, "Susie", "Susie", "Tyler", "/Assets/UserImages/4.png", 4)
                ,new User(5, "Lauz", "Laura", "Atkins", "/Assets/UserImages/5.png", 5)
                ,new User(6, "Botts", "Robin", "Bottington", "/Assets/UserImages/6.png", 6)
            };

            users = data;

            return data;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<User>> GetLoggedInUsersAsync()
        {
            await Task.CompletedTask;

            return LoggedInUsers();
        }

        /// <summary>
        /// Returns the user in collection with matching userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static User GetUser(int userId)
        {
            return users.SingleOrDefault(u => u.Id == userId);
        }
    }
}
