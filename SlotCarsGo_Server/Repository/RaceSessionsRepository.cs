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
    public class RaceSessionsRepository<T, DTO> : IRepositoryAsync<RaceSession, RaceSessionDTO>
        where T : RaceSession
        where DTO : RaceSessionDTO
    {
        public async Task<RaceSession> Delete(int id)
        {
            RaceSession car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.RaceSessions.FindAsync(id);
                if (car != null)
                {
                    db.RaceSessions.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<RaceSessionDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.ProjectTo<RaceSessionDTO>();
            }
        }

        public async Task<RaceSession> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceSessions.FindAsync(id);
            }
        }

        public IEnumerable<RaceSessionDTO> GetForId(int trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceSessions.Where(s => s.TrackId == trackId).ProjectTo<RaceSessionDTO>();
            }
        }

        public async Task<RaceSession> Insert(RaceSession car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = db.RaceSessions.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(int id, RaceSession car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(car).State = EntityState.Modified;

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

                return db.Entry(car).State;
            }
        }
    }
}