using System;

using SlotCarsGo.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Threading;
using AutoMapper;
using SlotCarsGo.Models.Racing;
using SlotCarsGo_Server.Models.DTO;

namespace SlotCarsGo
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);


/*
            Mapper.Initialize(cfg => {
                cfg.CreateMap<RaceSession, RaceSessionDTO>()
                    .ReverseMap()
                        .ForMember(src => src.DriverResults, opt => opt.Ignore())
                        .ForMember(src => src.RaceType, opt => opt.Ignore())
                        .ForMember(src => src.Track, opt => opt.Ignore());
                cfg.CreateMap<DriverResult, DriverResultDTO>()
                    .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.ApplicationUserId))
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.DriverId));
                cfg.CreateMap<Track, TrackDTO>()
                    .ForPath(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.BestLapTime == null ? "No Record set" : src.BestLapTime.ApplicationUser.UserName))
                    .ForPath(dest => dest.TrackRecord, opt => opt.MapFrom(src => src.BestLapTime == null ? new System.TimeSpan(0, 0, 59) : src.BestLapTime.LapTime.Time))
                    .ReverseMap()
                        .ForMember(src => src.BestLapTimeId, opt => opt.Ignore())
                        .ForMember(src => src.ApplicationUsers, opt => opt.Ignore())
                        .ForMember(src => src.Cars, opt => opt.Ignore());
                cfg.CreateMap<Car, CarDTO>()
                    .ForPath(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.BestLapTime == null ? "No Record set" : src.BestLapTime.ApplicationUser.UserName))
                    .ForPath(dest => dest.TrackRecord, opt => opt.MapFrom(src => src.BestLapTime == null ? new System.TimeSpan(0, 0, 59) : src.BestLapTime.LapTime.Time))
                    .ReverseMap()
                        .ForPath(src => src.BestLapTimeId, opt => opt.Ignore());
                cfg.CreateMap<RaceType, RaceTypeDTO>();
                cfg.CreateMap<Driver, DriverDTO>()
                    .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUser.Id))
                    .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                    .ForPath(dest => dest.ImageName, opt => opt.MapFrom(src => src.ApplicationUser.ImageName))
                    .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.Car))
                    .ReverseMap()
                        .ForMember(src => src.ApplicationUserId, opt => opt.MapFrom(dest => dest.UserId))
                        .ForPath(src => src.ApplicationUser.UserName, opt => opt.MapFrom(dest => dest.UserName))
                        .ForPath(src => src.ApplicationUser.ImageName, opt => opt.MapFrom(dest => dest.ImageName))
                        .ForMember(src => src.Car, opt => opt.MapFrom(dest => dest.SelectedCar));
                cfg.CreateMap<LapTime, LapTimeDTO>()
                    .ReverseMap()
                        .ForMember(src => src.DriverResult, opt => opt.Ignore());
            });
*/
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
                DispatcherHelper.Initialize();
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.MainViewModel), new Views.ShellPage());
        }
    }
}
