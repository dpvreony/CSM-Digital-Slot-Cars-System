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
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using AutoMapper;

namespace SlotRaceSessionsGo_Server.Controllers
{
    public class RaceSessionsController : ApiController
    {
        private RaceSessionsRepository<RaceSession, RaceSessionDTO> repo = new RaceSessionsRepository<RaceSession, RaceSessionDTO>();

        // GET: api/RaceSessions
        public IEnumerable<RaceSessionDTO> GetRaceSessions()
        {
            return repo.GetAllAsDTO();
        }

        // GET: api/RaceSessions/5
        [ResponseType(typeof(RaceSessionDTO))]
        public async Task<IHttpActionResult> GetRaceSession(string id)
        {
            RaceSession raceSession = await repo.GetById(id);
            if (raceSession == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RaceSessionDTO>(raceSession));
        }

        // PUT: api/RaceSessions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRaceSession(string id, RaceSession raceSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceSession.Id)
            {
                return BadRequest();
            }

            if (await repo.Update(id, raceSession) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RaceSessions
        [ResponseType(typeof(RaceSessionDTO))]
        public async Task<IHttpActionResult> PostRaceSession(RaceSessionDTO raceSessionDTO)
        {
            RaceSession raceSession = Mapper.Map<RaceSession>(raceSessionDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            raceSession = await repo.Insert(raceSession);

            return CreatedAtRoute("DefaultApi", new { id = raceSession.Id }, Mapper.Map<RaceSessionDTO>(raceSession));
        }

        // DELETE: api/RaceSessions/5
        [ResponseType(typeof(RaceSessionDTO))]
        public async Task<IHttpActionResult> DeleteRaceSession(string id)
        {
            RaceSession raceSession = await repo.Delete(id);
            if (raceSession == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RaceSessionDTO>(raceSession));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}