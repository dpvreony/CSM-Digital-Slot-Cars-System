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
    public static class CarsInGarageDataService
    {
        private static IEnumerable<Car> garage;

        private static IEnumerable<Car> CarsInGarage()
        {
            // TODO: Query logins 
            var data = new ObservableCollection<Car>
            {
                new Car("1", "Ferrari F50", "1.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
                ,new Car("2", "Bentley Continental GT3", "2.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
                ,new Car("3", "Ford Escort 1980 MKII", "3.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
                ,new Car("4", "Lancia Delta S4", "4.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
                ,new Car("5", "Volkswagon Polo WRC 2013", "5.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
                ,new Car("6", "Mini Countryman WRC 2012", "6.jpg", new TimeSpan(0,0,5), "Dave Armshaw")
            };

            garage = data;

            return data;
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Car>> GetCarsInGarageAsync()
        {
            await Task.CompletedTask;

            return CarsInGarage();
        }

        /// <summary>
        /// Returns the car in collection with matching carId.
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        public static Car GetCar(string carId)
        {
            if (garage == null)
            {
                CarsInGarage();
            }
            return garage.SingleOrDefault(c => c.CarID == carId);
        }
    }
}
