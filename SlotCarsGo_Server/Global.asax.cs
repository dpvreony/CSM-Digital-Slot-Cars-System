using AutoMapper;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => {
                cfg.CreateMap<RaceSession, RaceSessionDTO>();
                cfg.CreateMap<DriverResult, DriverResultDTO>();
                cfg.CreateMap<Track, TrackDTO>();
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<RaceType, RaceTypeDTO>();
            });
        }
    }
}
