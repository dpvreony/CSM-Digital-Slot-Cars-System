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
        private static IEnumerable<User> LoggedInUsers()
        {
            // TODO: Query logins 
            var data = new ObservableCollection<User>
            {
                new User(1, "Tyler", "Tim", "Tyler", "/Assets/UserImages/1.png", new Car(1, "Ferrari F50"))
/*                ,new User(2, "Cake", "Dave", "Armshaw", "/Assets/UserImages/2.png", new Car(2, "Bentley Continental GT3"))

                ,new User(3, "Bowes", "Adam", "Bowes", "/Assets/UserImages/3.png", new Car(3, "Ford Escort 1980 MKII"))
                ,new User(4, "Susie", "Susie", "Tyler", "/Assets/UserImages/4.png", new Car(4, "Lancia Delta S4"))
                ,new User(5, "Lauz", "Laura", "Atkins", "/Assets/UserImages/5.png", new Car(5, "Volkswagon Polo WRC 2013"))
                ,new User(6, "Botts", "Robin", "Bottington", "/Assets/UserImages/6.png", new Car(6, "Mini Countrman WRC 2012"))
*/
            };

            return data;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<User>> GetLoggedInUsersAsync()
        {
            await Task.CompletedTask;

            return LoggedInUsers();
        }
    }
}
