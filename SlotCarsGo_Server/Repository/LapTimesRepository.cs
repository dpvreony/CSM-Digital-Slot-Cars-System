using SlotCarsGo_Server.Models;
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
    public class LapTimesRepository<T> : IRepositoryAsync<LapTime> where T : LapTime
    {
        public async Task<LapTime> Delete(int id)
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

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<LapTime> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes;
            }
        }

        public async Task<LapTime> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.LapTimes.FindAsync(id);
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

        public async Task<EntityState> Update(int id, LapTime lapTime)
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