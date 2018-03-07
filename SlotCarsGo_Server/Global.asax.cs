using AutoMapper;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
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
            Database.SetInitializer<ApplicationDbContext>(null);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => {
                cfg.CreateMap<RaceSession, RaceSessionDTO>();
                cfg.CreateMap<DriverResult, DriverResultDTO>()
                    .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.ApplicationUserId))
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.DriverId));
                cfg.CreateMap<Track, TrackDTO>();
                cfg.CreateMap<Car, CarDTO>()
                    .ForPath(dest => dest.RecordHolder, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                    .ReverseMap()
                        .ForPath(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.RecordHolder));
                cfg.CreateMap<RaceType, RaceTypeDTO>();
                cfg.CreateMap<Driver, DriverDTO>()
                    .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.ApplicationUser.Id))
                    .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                    .ForMember(dest => dest.SelectedCar, opt => opt.MapFrom(src => src.Car))
                    .ReverseMap()
                        .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.UserId))
                        .ForPath(dest => dest.ApplicationUser.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.SelectedCar));
                cfg.CreateMap<LapTime, LapTimeDTO>();
            });
        }
    }
}
