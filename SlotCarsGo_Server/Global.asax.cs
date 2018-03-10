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
                        .ForMember(dest => dest.DriverResults, opt => opt.Ignore())
                        .ForMember(dest => dest.RaceType, opt => opt.Ignore())
                        .ForMember(dest => dest.Track, opt => opt.Ignore());
                cfg.CreateMap<DriverResult, DriverResultDTO>()
                    .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.ApplicationUserId))
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.DriverId));
                cfg.CreateMap<Track, TrackDTO>()
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.Ignore())
                        .ForMember(dest => dest.ApplicationUsers, opt => opt.Ignore())
                        .ForMember(dest => dest.Cars, opt => opt.Ignore());
                cfg.CreateMap<Car, CarDTO>()
                    .ForPath(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                    .ReverseMap()
                        .ForPath(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.RecordHolder));
                cfg.CreateMap<RaceType, RaceTypeDTO>();
                cfg.CreateMap<Driver, DriverDTO>()
                    .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUser.Id))
                    .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                    .ForPath(dest => dest.ImageName, opt => opt.MapFrom(src => src.ApplicationUser.ImageName))
                    .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.Car))
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId))
                        .ForPath(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForPath(dest => dest.ApplicationUser.ImageName, opt => opt.MapFrom(src => src.ImageName))
                        .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.SelectedCar));
                cfg.CreateMap<LapTime, LapTimeDTO>()
                    .ReverseMap()
                        .ForMember(dest => dest.DriverResult, opt => opt.Ignore());
            });
        }
    }
}
