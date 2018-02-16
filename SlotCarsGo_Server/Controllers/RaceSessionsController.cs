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
using SlotCarsGo_Server.Models;

namespace SlotCarsGo_Server.Controllers
{
    public class RaceSessionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RaceSessions
        public IQueryable<RaceSession> GetRaceSessions()
        {
            return db.RaceSessions;
        }

        // GET: api/RaceSessions/5
        [ResponseType(typeof(RaceSession))]
        public async Task<IHttpActionResult> GetRaceSession(int id)
        {
            RaceSession raceSession = await db.RaceSessions.FindAsync(id);
            if (raceSession == null)
            {
                return NotFound();
            }

            return Ok(raceSession);
        }

        // PUT: api/RaceSessions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRaceSession(int id, RaceSession raceSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceSession.Id)
            {
                return BadRequest();
            }

            db.Entry(raceSession).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceSessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RaceSessions
        [ResponseType(typeof(RaceSession))]
        public async Task<IHttpActionResult> PostRaceSession(RaceSession raceSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RaceSessions.Add(raceSession);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = raceSession.Id }, raceSession);
        }

        // DELETE: api/RaceSessions/5
        [ResponseType(typeof(RaceSession))]
        public async Task<IHttpActionResult> DeleteRaceSession(int id)
        {
            RaceSession raceSession = await db.RaceSessions.FindAsync(id);
            if (raceSession == null)
            {
                return NotFound();
            }

            db.RaceSessions.Remove(raceSession);
            await db.SaveChangesAsync();

            return Ok(raceSession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceSessionExists(int id)
        {
            return db.RaceSessions.Count(e => e.Id == id) > 0;
        }
    }
}