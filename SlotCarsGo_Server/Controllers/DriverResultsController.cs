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
    public class DriverResultsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DriverResults
        public IQueryable<DriverResult> GetDriverResults()
        {
            return db.DriverResults;
        }

        // GET: api/DriverResults/5
        [ResponseType(typeof(DriverResult))]
        public async Task<IHttpActionResult> GetDriverResult(int id)
        {
            DriverResult driverResult = await db.DriverResults.FindAsync(id);
            if (driverResult == null)
            {
                return NotFound();
            }

            return Ok(driverResult);
        }

        // PUT: api/DriverResults/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDriverResult(int id, DriverResult driverResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driverResult.Id)
            {
                return BadRequest();
            }

            db.Entry(driverResult).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverResultExists(id))
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

        // POST: api/DriverResults
        [ResponseType(typeof(DriverResult))]
        public async Task<IHttpActionResult> PostDriverResult(DriverResult driverResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DriverResults.Add(driverResult);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = driverResult.Id }, driverResult);
        }

        // DELETE: api/DriverResults/5
        [ResponseType(typeof(DriverResult))]
        public async Task<IHttpActionResult> DeleteDriverResult(int id)
        {
            DriverResult driverResult = await db.DriverResults.FindAsync(id);
            if (driverResult == null)
            {
                return NotFound();
            }

            db.DriverResults.Remove(driverResult);
            await db.SaveChangesAsync();

            return Ok(driverResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverResultExists(int id)
        {
            return db.DriverResults.Count(e => e.Id == id) > 0;
        }
    }
}