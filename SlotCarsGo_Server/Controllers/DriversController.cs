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
        private IRepositoryAsync<Driver, DriverDTO> repo = new DriversRepository<Driver, DriverDTO>();

        // GET: api/Drivers
        public IEnumerable<DriverDTO> GetDrivers()
        {
            return repo.GetAll();
        }

        // GET: api/Drivers/Track/5
        [Route("api/Drivers/Track/{trackId}")]
        public IEnumerable<DriverDTO> GetForTrack(string trackId)
        {
            return repo.GetForId(trackId);
        }

        // GET: api/Drivers/5
        [ResponseType(typeof(DriverDTO))]
        public async Task<IHttpActionResult> GetDriver(string id)
        {
            Driver driver = await repo.GetById(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<DriverDTO>(driver));
        }

        // PUT: api/Drivers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDriver(string id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.Id)
            {
                return BadRequest();
            }

            if (await repo.Update(id, driver) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Drivers
        [ResponseType(typeof(DriverDTO))]
        public async Task<IHttpActionResult> PostDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            driver = await repo.Insert(driver);

            return CreatedAtRoute("DefaultApi", new { id = driver.Id }, Mapper.Map<DriverDTO>(driver));
        }

        // DELETE: api/Drivers/5
        [ResponseType(typeof(DriverDTO))]
        public async Task<IHttpActionResult> DeleteDriver(string id)
        {
            Driver driver = await repo.Delete(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<DriverDTO>(driver));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}