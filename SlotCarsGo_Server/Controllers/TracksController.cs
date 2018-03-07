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
        private IRepositoryAsync<Track, TrackDTO> repo = new TracksRepository<Track, TrackDTO>();

        // GET: api/Tracks
        public IEnumerable<TrackDTO> GetTracks()
        {
            return repo.GetAll();
        }

        // GET: api/Tracks/5
        [ResponseType(typeof(TrackDTO))]
        public async Task<IHttpActionResult> GetTrack(string id)
        {
            Track track = await repo.GetById(id);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<TrackDTO>(track));
        }

        // PUT: api/Tracks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrack(string id, Track track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != track.Id)
            {
                return BadRequest();
            }

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