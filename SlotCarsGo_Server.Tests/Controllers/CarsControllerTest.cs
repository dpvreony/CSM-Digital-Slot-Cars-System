using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotCarsGo_Server;
using SlotCarsGo_Server.Controllers;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;

namespace SlotCarsGo_Server.Tests.Controllers
{
    [TestClass]
    public class CarsControllerTest
    {
        Car car;
        CarDTO carDto;

        [TestInitialize()]
        public void Initialize()
        {
            car = new Car();
            car.Id = "1";

        }
/*
        [TestMethod]
        public async void GetCarTest()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            CarDTO result = await controller.GetCar(1) as CarDTO;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("car1", result.ElementAt(0));
            Assert.AreEqual("car2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("car", result);
        }

        [TestMethod]
        public void PostCarTest()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            await controller.PostCar("car");

            // Assert
        }

        [TestMethod]
        public async void PutCarTest()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            await controller.PutCar(5, "car");

            // Assert
        }

        [TestMethod]
        public async void DeleteCarTest()
        {
            // Arrange
            CarsController controller = new CarsController();

            // Act
            await controller.DeleteCar(5);

            // Assert
        }
*/
    }
}
