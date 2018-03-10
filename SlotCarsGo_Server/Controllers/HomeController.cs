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

            string userId = this.User.Identity.GetUserId();
            ApplicationUser user = Request.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

            if (user != null)
            {
                HomeViewModel model = new HomeViewModel();
                model.Setup(user);
                return View(model);
            }

            return View();
        }

        //
        // POST: /Register
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(HomeViewModel registerModel)
        {
            if (registerModel.Secret != null)
            {
                TracksRepository<Track> tracksRepo = new TracksRepository<Track>();
                IEnumerable<Track> tracks = tracksRepo.GetForId(registerModel.Secret);
//                List<Track> results = tracksRepo.GetForId(registerModel.Secret).ToList();
                Track track = tracks.FirstOrDefault();
                if (track != null)
                {
                    string userId = this.User.Identity.GetUserId();
                    ApplicationUser user = Request.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);
                    track.ApplicationUsers.Add(user);
                    EntityState state = await tracksRepo.Update(track.Id, track);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "That track secret was not found, try again.");
                }
            }

            return Index();
        }

        //
        // POST: /Join
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Join(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                IRepositoryAsync<Track> tracksRepo = new TracksRepository<Track>();
                Track track = tracksRepo.GetAll().Where(t => t.Secret == model.Secret).FirstOrDefault();
                track.ApplicationUsers.Add(model.User);
                EntityState state = await tracksRepo.Update(track.Id, track);
            }

            return Index();
        }
    }
}
