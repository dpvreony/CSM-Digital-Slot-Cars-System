using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models.DTO;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace SlotCarsGo_Server.Controllers
{
    public class LapTimesController : ApiController
    {
        private IRepositoryAsync<LapTime> repo = new LapTimesRepository<LapTime>();

        // GET: api/LapTimes
        public IQueryable<LapTimeDTO> GetLapTimes()
        {
            return repo.GetAll().ProjectTo<LapTimeDTO>();
        }

        // GET: api/LapTimes/5
        [ResponseType(typeof(LapTimeDTO))]
        public async Task<IHttpActionResult> GetLapTime(int id)
        {
            LapTime lapTime = await repo.GetById(id);
            if (lapTime == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<LapTimeDTO>(lapTime));
        }

        // PUT: api/LapTimes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLapTime(int id, LapTime lapTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lapTime.Id)
            {
                return BadRequest();
            }

            if (await repo.Update(id, lapTime) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LapTimes
        [ResponseType(typeof(LapTimeDTO))]
        public async Task<IHttpActionResult> PostLapTime(LapTimeDTO lapTimeDTO)
        {
            LapTime lapTime = Mapper.Map<LapTime>(lapTimeDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            lapTime = await repo.Insert(lapTime);

            return CreatedAtRoute("DefaultApi", new { id = lapTime.Id }, Mapper.Map<LapTimeDTO>(lapTime));
        }

        // DELETE: api/LapTimes/5
        [ResponseType(typeof(LapTimeDTO))]
        public async Task<IHttpActionResult> DeleteLapTime(int id)
        {
            LapTime lapTime = await repo.Delete(id);
            if (lapTime == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<LapTimeDTO>(lapTime));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}