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
    public class RaceSessionsRepository<T> : IRepositoryAsync<RaceSession>
    {
        public async Task<RaceSession> Delete(string id)
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

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<RaceSession> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions;
            }
        }

        public IEnumerable<RaceSessionDTO> GetAllAsDTO(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Where(rs => rs.TrackId == trackId).ProjectTo<RaceSessionDTO>();
            }
        }

        public async Task<RaceSession> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceSessions.FindAsync(id);
            }
        }

        public IEnumerable<RaceSession> GetFor(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Where(s => s.TrackId == trackId);
            }
        }

        public async Task<RaceSession> Insert(RaceSession raceSession)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceSession.Id = string.IsNullOrEmpty(raceSession.Id) ? Guid.NewGuid().ToString() : raceSession.Id;
                raceSession = db.RaceSessions.Add(raceSession);
                await db.SaveChangesAsync();
            }

            return raceSession;
        }

        public async Task<EntityState> Update(string id, RaceSession raceSession)
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