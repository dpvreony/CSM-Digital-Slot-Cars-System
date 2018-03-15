using Microsoft.AspNet.Identity;
using SlotCarsGo_Server.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlotCarsGo_Server.Models.DTO;

namespace SlotCarsGo_Server.Models.ViewModels
{
    public class HomeViewModel
    {
        private IEnumerable<Driver> confirmedDrivers;
        private IEnumerable<int> controllers = new List<int> { 1, 2, 3, 4, 5, 6 };

        public ApplicationUser User { get; set; }

        // Last session details
        public RaceSession LastSession { get; set; }
        public DriverResult LastDriverResult { get; set; }
        public Track LastTrack { get; set; }

        // Join race form
        private IEnumerable<Track> MyTracks { get; set; }
        public IEnumerable<Car> AvailableCars { get; set; }
        public IEnumerable<int> AvailableControllerIds { get; set; }

        public List<SelectListItem> MyTracksListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableCarsListItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableControllersListItems { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Track")]
        public string SelectedTrackId { get; set; }
        [Required]
        [Display(Name = "Car")]
        public string SelectedCarId { get; set; }
        [Required]
        [Display(Name = "Controller")]
        public string SelectedControllerId { get; set; }


        public HomeViewModel()
        {
        }

        public void Setup(ApplicationUser loggedInUser)
        {
            this.User = loggedInUser;
            if (this.User.Tracks.Count > 0)
            {
                // Try to find last session details
                this.LastDriverResult = this.User.DriverResults?.OrderByDescending(dr => dr.Session.EndTime).FirstOrDefault();
                this.LastSession = this.LastDriverResult?.Session;
                this.LastTrack = this.LastSession == null ? this.User.Tracks?.FirstOrDefault() : this.LastSession?.Track;

                // Populate join race form choices
                this.MyTracks = this.User.Tracks.ToList();

                DriversRepository<Driver, DriverDTO> driversRepo = new DriversRepository<Driver, DriverDTO>();
                this.confirmedDrivers = driversRepo.GetForTrack(LastTrack.Id);
                
                this.AvailableCars = (from car in this.LastTrack.Cars
                                      where !(this.confirmedDrivers.Any(driver => driver.CarId == car.Id))
                                      select car).ToList();

                this.AvailableControllerIds = (from controller in this.controllers
                                               where !(this.confirmedDrivers.Any(driver => driver.ControllerId == controller))
                                               select controller).ToList();

                // Populate dropdown menu select item lists
                foreach (Track track in this.MyTracks)
                {
                    MyTracksListItems.Add(new SelectListItem { Text = track.Name, Value = track.Id });
                }
                foreach (Car car in this.AvailableCars)
                {
                    AvailableCarsListItems.Add(new SelectListItem { Text = car.Name, Value = car.Id });
                }
                foreach (int controller in this.AvailableControllerIds)
                {
                    AvailableControllersListItems.Add(new SelectListItem { Text = controller.ToString(), Value = controller.ToString() });
                }
            }
        }
    }

    public class RegisterTrackViewModel
    {
        public ApplicationUser User { get; set; }

        // Register with a Track
        [Required]
        [Display(Name = "Track Secret")]
        public string Secret { get; set; }

}
}