using AutoMapper;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SlotCarsGo_Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
//            if (ConfigurationManager.AppSettings["BuildType"] == "RELEASE")
//            {
                Database.SetInitializer<ApplicationDbContext>(null); // Prevents EF creating a new DB
//            }

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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
                    .ForPath(dest => dest.TrackRecord, opt => opt.MapFrom(src => src.BestLapTime == null ? new System.TimeSpan(0,0,59) : src.BestLapTime.LapTime.Time))
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
        }
    }
}
