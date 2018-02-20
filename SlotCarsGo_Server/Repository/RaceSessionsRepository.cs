using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SlotCarsGo_Server.Repository
{
    public class RaceSessionsRepository<T> : IRepositoryAsync<RaceSession> where T : RaceSession
    {
        public async Task<RaceSession> Delete(int id)
        {
            RaceSession raceSession;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceSession = await db.RaceSessions.FindAsync(id);
                if (raceSession != null)
                {
                    db.RaceSessions.Remove(raceSession);
                    await db.SaveChangesAsync();
                }
            }

            return raceSession;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<RaceSession> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions;
            }
        }

        public async Task<RaceSession> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceSessions.FindAsync(id);
            }
        }

        public async Task<RaceSession> Insert(RaceSession raceSession)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceSession = db.RaceSessions.Add(raceSession);
                await db.SaveChangesAsync();
            }

            return raceSession;
        }

        public async Task<EntityState> Update(int id, RaceSession raceSession)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(raceSession).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.RaceSessions.Count(e => e.Id == id) == 0)
                    {
                        return EntityState.Unchanged;
                    }
                    else
                    {
                        throw;
                    }
                }

                return db.Entry(raceSession).State;
            }
        }
    }
}