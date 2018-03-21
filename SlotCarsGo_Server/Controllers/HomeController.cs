using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Models.ViewModels;
using SlotCarsGo_Server.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            try
            {
                string userId = this.User.Identity.GetUserId();
                ApplicationUser user = Request.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                if (user.Tracks.Count == 0)
                {
                    return RedirectToAction("RegisterTrack");
                }
                else
                {
                    HomeViewModel homeModel = new HomeViewModel();
                    homeModel.Setup(user);
                    return View(homeModel);
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult RegisterTrack()
        {
            string userId = this.User.Identity.GetUserId();
            ApplicationUser user = Request.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

            RegisterTrackViewModel registerModel = new RegisterTrackViewModel();
            registerModel.User = user;
            return View(registerModel);
        }


        public ActionResult About()
        {
            ViewBag.Title = "About";

            return View();
        }

        // POST: /Register
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterTrackPost(RegisterTrackViewModel registerModel)
        {
            if (registerModel.Secret != null)
            {
                string userId = this.User.Identity.GetUserId();

                TracksRepository<Track> tracksRepo = new TracksRepository<Track>();
                IEnumerable<Track> tracks = tracksRepo.GetFor(registerModel.Secret);
                Track track = tracks.FirstOrDefault() as Track;
                if (track != null)
                {
                    EntityState state = await tracksRepo.RegisterUser(track.Id, userId);
                    if (state == EntityState.Modified)
                    {
                        return RedirectToAction("Index", "Garage");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Sorry, something went wrong - please try again.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "That track secret was not found, try again.");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Join
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JoinRace(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = this.User.Identity.GetUserId();
                DriversRepository<Driver, DriverDTO> driversRepo = new DriversRepository<Driver, DriverDTO>();
                Driver driver = driversRepo.GetForUser(userId);

                if (driver == null)
                {
                    driver = new Driver()
                    {
                        ApplicationUserId = userId,
                        CarId = model.SelectedCarId,
                        ControllerId = Convert.ToInt32(model.SelectedControllerId),
                        TrackId = model.SelectedTrackId
                    };

                    driver = await driversRepo.Insert(driver);
                }
                else
                {
                    driver.CarId = model.SelectedCarId;
                    driver.ControllerId = Convert.ToInt32(model.SelectedControllerId);
                    driver.TrackId = model.SelectedTrackId;
                    EntityState state = await driversRepo.Update(driver.Id, driver);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
