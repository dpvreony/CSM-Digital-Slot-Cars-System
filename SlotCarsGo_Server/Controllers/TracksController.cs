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
using SlotCarsGo_Server.Models.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace SlotCarsGo_Server.Controllers
{
    public class TracksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tracks
        public IQueryable<TrackDTO> GetTracks()
        {
            var tracks = from t in db.Tracks
                         select new TrackDTO()
                         {
                             Id = t.Id,
                             Name = t.Name,
                             Length = t.Length
                         };

            return db.Tracks.ProjectTo<TrackDTO>();
        }

        // GET: api/Tracks/5
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> GetTrack(int id)
        {
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(track);
        }

        // PUT: api/Tracks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrack(int id, Track track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != track.Id)
            {
                return BadRequest();
            }

            db.Entry(track).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
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

        // POST: api/Tracks
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> PostTrack(Track track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tracks.Add(track);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = track.Id }, track);
        }

        // DELETE: api/Tracks/5
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> DeleteTrack(int id)
        {
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            db.Tracks.Remove(track);
            await db.SaveChangesAsync();

            return Ok(track);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrackExists(int id)
        {
            return db.Tracks.Count(e => e.Id == id) > 0;
        }
    }
}