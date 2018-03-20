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
            DriversRepository<Driver, DriverDTO> driversRepo = new DriversRepository<Driver, DriverDTO>();
            Driver driver = driversRepo.GetForUser(this.User.Id);

            // Try to find last session details
            this.LastTrack = this.User.Tracks.FirstOrDefault();

            // Populate join race form choices
            this.MyTracks = this.User.Tracks.ToList();

            this.confirmedDrivers = driversRepo.GetForTrack(LastTrack.Id).Where(d => d.ApplicationUserId != this.User.Id);
                
            this.AvailableCars = (from car in this.LastTrack.Cars
                                    where car.Selectable == true && !(this.confirmedDrivers.Any(d => d.CarId == car.Id))
                                    select car).ToList();

            this.AvailableControllerIds = (from controller in this.controllers
                                            where !(this.confirmedDrivers.Any(d => d.ControllerId == controller))
                                            select controller).ToList();

            // Populate dropdown menu select item lists
            foreach (Track track in this.MyTracks)
            {
                if (driver == null || driver.TrackId != track.Id)
                {
                    MyTracksListItems.Add(new SelectListItem { Text = track.Name, Value = track.Id });
                }
                else
                {
                    MyTracksListItems.Add(new SelectListItem { Text = track.Name, Value = track.Id, Selected = true });
                }
            }
            foreach (Car car in this.AvailableCars)
            {
                if (driver == null || driver.CarId != car.Id)
                {
                    AvailableCarsListItems.Add(new SelectListItem { Text = car.Name, Value = car.Id });
                }
                else
                {
                    AvailableCarsListItems.Add(new SelectListItem { Text = car.Name, Value = car.Id, Selected = true });
                }
            }
            foreach (int controller in this.AvailableControllerIds)
            {
                if (driver == null || driver.ControllerId != controller)
                {
                    AvailableControllersListItems.Add(new SelectListItem { Text = controller.ToString(), Value = controller.ToString() });
                }
                else
                {
                    AvailableControllersListItems.Add(new SelectListItem { Text = controller.ToString(), Value = controller.ToString(), Selected = true });
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