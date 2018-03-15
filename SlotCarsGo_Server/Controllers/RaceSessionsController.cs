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
        private RaceSessionsRepository<RaceSession> repo = new RaceSessionsRepository<RaceSession>();

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