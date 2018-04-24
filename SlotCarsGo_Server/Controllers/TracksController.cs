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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models.DTO;

namespace SlotTracksGo_Server.Controllers
{
    public class TracksController : ApiController
    {
        private TracksRepository<Track> repo = new TracksRepository<Track>();

        // GET: api/Tracks/5
        [HttpGet]
        [ResponseType(typeof(TrackDTO))]
        [Route("api/Tracks/{secret}")]
        public async Task<IHttpActionResult> GetTrack(string secret)
        {
            Track track = await repo.GetBySecret(secret);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<TrackDTO>(track));
        }

        // PUT: api/Tracks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrack(string id, TrackDTO trackDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trackDTO.Id)
            {
                return BadRequest();
            }

            Track track = await repo.GetById(id);
            track.Name = trackDTO.Name;
            track.Length = trackDTO.Length;

            if (await repo.Update(id, track) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tracks
        [ResponseType(typeof(TrackDTO))]
        public async Task<IHttpActionResult> PostTrack(TrackDTO trackDTO)
        {
            Track track = Mapper.Map<Track>(trackDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            track = await repo.Insert(track);

            return CreatedAtRoute("DefaultApi", new { id = track.Id }, Mapper.Map<TrackDTO>(track));
        }

        // DELETE: api/Tracks/5
        [ResponseType(typeof(TrackDTO))]
        public async Task<IHttpActionResult> DeleteTrack(string id)
        {
            Track track = await repo.Delete(id);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<TrackDTO>(track));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}