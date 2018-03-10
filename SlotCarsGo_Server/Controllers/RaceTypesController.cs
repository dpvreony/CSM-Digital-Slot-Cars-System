using AutoMapper;
using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Repository;
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

namespace SlotRaceTypesGo_Server.Controllers
{
    public class RaceTypesController : ApiController
    {
        private IRepositoryAsync<RaceType> repo = new RaceTypesRepository<RaceType>();

        // GET: api/RaceTypes
        public IEnumerable<RaceTypeDTO> GetRaceTypes()
        {
            return repo.GetAll().ProjectTo<RaceTypeDTO>();
        }

        // GET: api/RaceTypes/5
        [ResponseType(typeof(RaceTypeDTO))]
        public async Task<IHttpActionResult> GetRaceType(string id)
        {
            RaceType raceType = await repo.GetById(id);
            if (raceType == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RaceTypeDTO>(raceType));
        }

        // PUT: api/RaceTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRaceType(string id, RaceType raceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceType.Id)
            {
                return BadRequest();
            }

            if (await repo.Update(id, raceType) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RaceTypes
        [ResponseType(typeof(RaceTypeDTO))]
        public async Task<IHttpActionResult> PostRaceType(RaceType raceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            raceType = await repo.Insert(raceType);

            return CreatedAtRoute("DefaultApi", new { id = raceType.Id }, Mapper.Map<RaceTypeDTO>(raceType));
        }

        // DELETE: api/RaceTypes/5
        [ResponseType(typeof(RaceTypeDTO))]
        public async Task<IHttpActionResult> DeleteRaceType(string id)
        {
            RaceType raceType = await repo.Delete(id);
            if (raceType == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RaceTypeDTO>(raceType));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}