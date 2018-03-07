using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SlotCarsGo_Server.Repository
{
    public class LapTimesRepository<T, DTO> : IRepositoryAsync<LapTime, LapTimeDTO>
        where T : LapTime
        where DTO : LapTimeDTO
    {
        public async Task<LapTime> Delete(string id)
        {
            LapTime lapTime;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                lapTime = await db.LapTimes.FindAsync(id);
                if (lapTime != null)
                {
                    db.LapTimes.Remove(lapTime);
                    await db.SaveChangesAsync();
                }
            }

            return lapTime;
        }

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<LapTimeDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes.ProjectTo<LapTimeDTO>();
            }
        }

        public async Task<LapTime> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.LapTimes.FindAsync(id);
            }
        }

        public IEnumerable<LapTimeDTO> GetForId(string driverResultId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                DriverResult dr = db.DriverResults.Find(driverResultId);

                return db.LapTimes.Where(lt => lt.RaceSessionId == dr.SessionId && lt.DriverId == dr.ApplicationUserId).ProjectTo<LapTimeDTO>();
            }
        }

        public async Task<LapTime> Insert(LapTime lapTime)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                lapTime = db.LapTimes.Add(lapTime);
                await db.SaveChangesAsync();
            }

            return lapTime;
        }

        public async Task<EntityState> Update(string id, LapTime lapTime)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(lapTime).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.LapTimes.Count(e => e.Id == id) == 0)
                    {
                        return EntityState.Unchanged;
                    }
                    else
                    {
                        throw;
                    }
                }

                return db.Entry(lapTime).State;
            }
        }
    }
}