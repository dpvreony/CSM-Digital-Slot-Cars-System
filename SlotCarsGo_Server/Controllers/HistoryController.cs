using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        // GET: /History/Index
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
                    /*
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
                    */
                    return View();

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}