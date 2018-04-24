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
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Repository;

namespace SlotDriverResultsGo_Server.Controllers
{
    public class DriverResultResultsController : ApiController
    {
        public class DriverResultsController : ApiController
        {
            private IRepositoryAsync<DriverResult> repo = new DriverResultsRepository<DriverResult>();

            // POST: api/DriverResults
            [ResponseType(typeof(DriverResultDTO))]
            public async Task<IHttpActionResult> PostDriverResult(DriverResultDTO driverResultDTO)
            {
                DriverResult driverResult = Mapper.Map<DriverResult>(driverResultDTO);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                driverResult = await repo.Insert(driverResult);

                // Remove this driver from registered drivers
                DriversRepository<Driver, DriverDTO> driversRepo = new DriversRepository<Driver, DriverDTO>();
                Driver driver = driversRepo.GetForUser(driverResult.ApplicationUserId);
                if (driver != null)
                {
                    await driversRepo.Delete(driver.Id);
                }

                return CreatedAtRoute("DefaultApi", new { id = driverResult.Id }, Mapper.Map<DriverResultDTO>(driverResult));
            }

            // DELETE: api/DriverResults/5
            [ResponseType(typeof(DriverResultDTO))]
            public async Task<IHttpActionResult> DeleteDriverResult(string id)
            {
                DriverResult driverResult = await repo.Delete(id);
                if (driverResult == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<DriverResultDTO>(driverResult));
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }
        }
    }
}