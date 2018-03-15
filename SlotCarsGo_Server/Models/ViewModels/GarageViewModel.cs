using AutoMapper;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlotCarsGo_Server.Models.ViewModels
{
    public class GarageViewModel
    {
        public GarageViewModel()
        {
        }

        public ApplicationUser User { get; set; }
        public ChangeTrackViewModel ChangeTrackViewModel { get; set; }
        public ChangeCarViewModel ChangeCarViewModel { get; set; }
        public NewCarViewModel NewCarViewModel { get; set; }
        public EditCarViewModel EditCarViewModel { get; set; }
        public DeleteCarViewModel DeleteCarViewModel { get; set; }

        public string SelectedTrackId { get; set; }
        public Track SelectedTrack { get; set; }
        public string TrackRecord { get; set; }
        public string TrackRecordHolder { get; set; }
        private List<TrackDTO> MyTracks { get; set; } = new List<TrackDTO>();
        public List<SelectListItem> MyTracksListItems { get; set; } = new List<SelectListItem>();

        public string SelectedCarId { get; set; }
        public CarDTO SelectedCar { get; set; }
        public List<BestLapTime> BestLapTimesForCar { get; set; }
        public List<CarDTO> CarsInGarage { get; set; } = new List<CarDTO>();
        public List<SelectListItem> CarsInGarageListItems { get; set; } = new List<SelectListItem>();

        public void Setup(ApplicationUser loggedInUser)
        {
            this.User = loggedInUser;

            // Set Track Values
            if (string.IsNullOrEmpty(SelectedTrackId))
            {
                this.SelectedTrack = this.User.Tracks.FirstOrDefault();
                this.SelectedTrackId = this.SelectedTrack.Id;
            }
            else
            {
                this.SelectedTrack = this.User.Tracks.Where(t => t.Id == this.SelectedTrackId).FirstOrDefault();
            }
            this.TrackRecord = this.SelectedTrack.BestLapTime == null ? "No Record Set" : this.SelectedTrack.BestLapTime.LapTime.Time.ToString(@"m\:ss\.fff");
            this.TrackRecordHolder = this.SelectedTrack.BestLapTime == null ? "No Record Set" : this.SelectedTrack.BestLapTime.ApplicationUser.UserName;
            foreach (Track track in this.User.Tracks.ToList())
            {
                TrackDTO trackDTO = Mapper.Map<Track, TrackDTO>(track);
                this.MyTracks.Add(trackDTO);
                this.MyTracksListItems.Add(new SelectListItem() { Text = trackDTO.Name, Value = trackDTO.Id });
            }

            // Set Cars in Selected Track values
            var cars = this.SelectedTrack.Cars
                .Where(c => c.TrackId == this.SelectedTrackId && c.BestLapTime != null)
                .OrderBy(c => c.BestLapTime)
                .Concat(
                    this.SelectedTrack.Cars
                    .Where(c => c.TrackId == this.SelectedTrackId && c.BestLapTime == null))
                    .OrderBy(c => c.Name)
                    .ToList();
            foreach (Car car in cars)
            {
                CarDTO carDTO = Mapper.Map<Car, CarDTO>(car);
                this.CarsInGarage.Add(carDTO);
                this.CarsInGarageListItems.Add(new SelectListItem() { Text = carDTO.Name, Value = carDTO.Id });
            }

            Car tempCar = string.IsNullOrEmpty(SelectedCarId)
                ? cars.FirstOrDefault()
                : cars.Where(c => c.Id == this.SelectedCarId).FirstOrDefault();

            this.SelectedCarId = tempCar.Id;
            this.SelectedCar = Mapper.Map<Car, CarDTO>(tempCar);
            this.BestLapTimesForCar = tempCar.BestLapTimes.OrderBy(l => l.LapTime.Time).ToList();

            // Set up child View Models
            this.ChangeTrackViewModel = new ChangeTrackViewModel { SelectedTrackId = this.SelectedTrackId, MyTracksListItems = this.MyTracksListItems };
            this.ChangeCarViewModel = new ChangeCarViewModel { SelectedCarId = this.SelectedCarId, CarsInGarageListItems = this.CarsInGarageListItems };
            this.NewCarViewModel = new NewCarViewModel { MyTracksListItems = this.MyTracksListItems };
            this.EditCarViewModel = new EditCarViewModel { CarsInGarageListItems = this.CarsInGarageListItems };
            this.DeleteCarViewModel = new DeleteCarViewModel { CarsInGarageListItems = CarsInGarageListItems };

            // Set selected Car Values
            this.EditCarViewModel.SetCarToEdit(tempCar);
            this.DeleteCarViewModel.SetCarToDelete(tempCar);
        }
    }

    public class ChangeTrackViewModel
    {
        public ChangeTrackViewModel()
        {
        }

        [Required]
        [Display(Name = "Track")]
        public string SelectedTrackId { get; set; }

        public List<SelectListItem> MyTracksListItems { get; set; }
    }

    public class ChangeCarViewModel
    {
        public ChangeCarViewModel()
        {
        }

        [Required]
        [Display(Name = "Car")]
        public string SelectedCarId { get; set; }

        public List<SelectListItem> CarsInGarageListItems { get; set; }
    }

    public class NewCarViewModel
    {
        public NewCarViewModel()
        {
        }

        [Required]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileWrapper Image { get; set; }

        [Required]
        [Display(Name = "Track")]
        public string TrackId { get; set; }

        public List<SelectListItem> MyTracksListItems { get; set; }

    }

    public class EditCarViewModel
    {
        public EditCarViewModel()
        {
        }

        [Required]
        [Display(Name = "Car")]
        public string SelectedCarToEditId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileWrapper Image { get; set; }

        public List<SelectListItem> CarsInGarageListItems { get; set; }

        public void SetCarToEdit(Car carToEdit)
        {
            this.SelectedCarToEditId = carToEdit.Id;
            this.Name = carToEdit.Name;
        }
    }

    public class DeleteCarViewModel
    {
        public DeleteCarViewModel()
        {
        }

        [Required]
        [Display(Name = "Car")]
        public string SelectedCarToDeleteId { get; set;}

        public List<SelectListItem> CarsInGarageListItems { get; set; }

        public void SetCarToDelete(Car carToDelete)
        {
            this.SelectedCarToDeleteId = carToDelete.Id;
        }
    }
}