using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;

namespace SlotDriversGo_Server.Controllers
{
    public class DriversController : ApiController
    {
        private DriversRepository<Driver, DriverDTO> repo = new DriversRepository<Driver, DriverDTO>();

        // GET: api/Drivers/5
        [Route("api/Drivers/{trackId}")]
        public IEnumerable<DriverDTO> GetDrivers(string trackId)
        {
            return repo.GetAllAsDTO(trackId);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}