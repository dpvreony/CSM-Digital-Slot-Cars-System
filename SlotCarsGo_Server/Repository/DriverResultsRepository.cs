using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SlotCarsGo_Server.Repository
{
    public class DriverResultsRepository<T, DTO> : IRepositoryAsync<DriverResult, DriverResultDTO> 
        where T : DriverResult 
        where DTO: DriverResultDTO
    {
        public async Task<DriverResult> Delete(string id)
        {
            DriverResult driverResult;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                driverResult = await db.DriverResults.FindAsync(id);
                if (driverResult != null)
                {
                    db.DriverResults.Remove(driverResult);
                    await db.SaveChangesAsync();
                }
            }

            return driverResult;
        }

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.DriverResults.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<DriverResultDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.DriverResults.ProjectTo<DriverResultDTO>();
            }
        }

        public async Task<DriverResult> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.DriverResults.FindAsync(id);
            }
        }

        public IEnumerable<DriverResultDTO> GetForId(string sessionId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.DriverResults.Where(dr => dr.SessionId == sessionId).ProjectTo<DriverResultDTO>();
            }
        }

        public async Task<DriverResult> Insert(DriverResult driverResult)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                driverResult = db.DriverResults.Add(driverResult);
                await db.SaveChangesAsync();
            }

            return driverResult;
        }

        public async Task<EntityState> Update(string id, DriverResult driverResult)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(driverResult).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.DriverResults.Count(e => e.Id == id) == 0)
                    {
                        return EntityState.Unchanged;
                    }
                    else
                    {
                        throw;
                    }
                }

                return db.Entry(driverResult).State;
            }
        }
    }
}