using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Models.ViewModels;
using SlotCarsGo_Server.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    [Authorize]
    public class GarageController : Controller
    {
        // GET: /Garage/Index
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Manage Garage";
            try
            {
                string userId = this.User.Identity.GetUserId();
                ApplicationUser user = Request.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                if (user?.Tracks.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    GarageViewModel garageModel = new GarageViewModel();

                    var trackId = TempData["TrackId"];
                    if (trackId != null)
                    {
                        garageModel.SelectedTrackId = trackId.ToString();
                    }

                    var carId = TempData["CarId"];
                    if (carId != null)
                    {
                        garageModel.SelectedCarId = carId.ToString();
                    }

                    garageModel.Setup(user);

                    return View(garageModel);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: /Garage/AddCar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCar(CreateCarViewModel newCarViewModel)
        {
            if (ModelState.IsValid)
            {
                string carId = Guid.NewGuid().ToString();
                Car car = new Car()
                {
                    Id = carId,
                    Name = $"{newCarViewModel.Make} {newCarViewModel.Model}",
                    TrackId = newCarViewModel.TrackId
                };

                string carsImagesPath = HttpContext.Server.MapPath("~/Content/Images/Cars");
                bool fileNotFound = true;
                // Add uploaded image if selected, or use default
                if (Request.Files.Count > 0)
                {
                    var postedFile = Request.Files[0];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        fileNotFound = false;
                        string extension = Path.GetExtension(postedFile.FileName);
                        string carFileName = $"{carId}{extension}";
                        car.ImageName = carFileName;
                        string saveToPath = Path.Combine(carsImagesPath, carFileName);
                        postedFile.SaveAs(saveToPath);
                    }
                }

                if (fileNotFound)
                {
                    var defaultCarFileToUsePath = Path.Combine(carsImagesPath, "0.jpg");
                    if (System.IO.File.Exists(defaultCarFileToUsePath))
                    {
                        string defaultCarImageWithUniqueCarId = $"{carId}.jpg";
                        car.ImageName = defaultCarImageWithUniqueCarId;
                        string defaultSaveToPath = Path.Combine(carsImagesPath, defaultCarImageWithUniqueCarId);
                        byte[] bytes = System.IO.File.ReadAllBytes(defaultCarFileToUsePath);
                        System.IO.File.WriteAllBytes(defaultSaveToPath, bytes);
                    }
                }

                try
                {
                    CarsRepository<Car, CarDTO> carsRepo = new CarsRepository<Car, CarDTO>();
                    car = await carsRepo.Insert(car);
                    TempData["CarId"] = car.Id;
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Failed to save new car.");
                }
            }

            return RedirectToAction("Index");
        }

        // POST: /Garage/EditCarImage
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCar(EditCarViewModel editCarViewModel)
        {
            if (ModelState.IsValid)
            { 
                bool changed = false;
                CarsRepository<Car, CarDTO> carsRepo = new CarsRepository<Car, CarDTO>();
                Car car = await carsRepo.GetById(editCarViewModel.SelectedCarToEditId);

                // Add image if selected
                if (Request.Files.Count > 0)
                {
                    var postedFile = Request.Files[0];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string carsImagesPath = HttpContext.Server.MapPath("~/Content/Images/Cars");
                        string extension = Path.GetExtension(postedFile.FileName);
                        string carFileName = $"{editCarViewModel.SelectedCarToEditId}{extension}";
                        string saveToPath = Path.Combine(carsImagesPath, carFileName);
                        try
                        {
                            postedFile.SaveAs(saveToPath);
                            car.ImageName = carFileName;
                            changed = true;
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to save new car image.");
                        }
                    }
                }


                if (editCarViewModel.Name != car.Name)
                {
                    car.Name = editCarViewModel.Name;
                    changed = true;
                }

                if (changed)
                {
                    await carsRepo.Update(car.Id, car);
                    TempData["CarId"] = car.Id;
                }
            }

            return RedirectToAction("Index");
        }

        // POST: /Garage/DeleteCar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCar(DeleteCarViewModel deleteCarViewModel)
        {
            if (ModelState.IsValid)
            {
                CarsRepository<Car, CarDTO> carsRepo = new CarsRepository<Car, CarDTO>();
                try
                {

                    Car car = await carsRepo.Remove(deleteCarViewModel.SelectedCarToDeleteId);
                    if (car == null) ModelState.AddModelError(string.Empty, "Failed to remove car from garage.");
                    string carsImagesPath = HttpContext.Server.MapPath("~/Content/Images/Cars");
                    string fileNamePath = Path.Combine(carsImagesPath, car.ImageName);
                    if (System.IO.File.Exists(fileNamePath))
                    {
                        System.IO.File.Delete(fileNamePath);
                    }
                }
                catch(Exception)
                {
                    ModelState.AddModelError(string.Empty, "Failed to remove car.");
                }
            }

            return RedirectToAction("Index");
        }

        // POST: /Garage/ChangeCar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTrack(ChangeTrackViewModel garageModel)
        {
            if (ModelState.IsValid)
            {
                TempData["TrackId"] = garageModel.SelectedTrackId;
            }

            return RedirectToAction("Index");
        }

        // POST: /Garage/ChangeCar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeCar(ChangeCarViewModel garageModel)
        {
            if (ModelState.IsValid)
            {
                TempData["CarId"] = garageModel.SelectedCarId;
            }

            return RedirectToAction("Index");
        }
    }
}
