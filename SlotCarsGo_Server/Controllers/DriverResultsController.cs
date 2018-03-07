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
            private IRepositoryAsync<DriverResult, DriverResultDTO> repo = new DriverResultsRepository<DriverResult, DriverResultDTO>();

            // GET: api/DriverResults
            public IEnumerable<DriverResultDTO> GetDriverResults()
            {
                return repo.GetAll();
            }

            // GET: api/DriverResults/5
            [ResponseType(typeof(DriverResultDTO))]
            public async Task<IHttpActionResult> GetDriverResult(string id)
            {
                DriverResult driverResult = await repo.GetById(id);
                if (driverResult == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<DriverResultDTO>(driverResult));
            }

            // PUT: api/DriverResults/5
            [ResponseType(typeof(void))]
            public async Task<IHttpActionResult> PutDriverResult(string id, DriverResult driverResult)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != driverResult.Id)
                {
                    return BadRequest();
                }

                if (await repo.Update(id, driverResult) != EntityState.Modified)
                {
                    return NotFound();
                }

                return StatusCode(HttpStatusCode.NoContent);
            }

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