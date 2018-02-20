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
    public class RaceTypesRepository<T> : IRepositoryAsync<RaceType> where T : RaceType
    {
        public async Task<RaceType> Delete(int id)
        {
            RaceType raceType;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceType = await db.RaceTypes.FindAsync(id);

                if (raceType != null)
                {
                    db.RaceTypes.Remove(raceType);
                    await db.SaveChangesAsync();
                }
            }

            return raceType;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<RaceType> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes;
            }
        }

        public async Task<RaceType> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceTypes.FindAsync(id);
            }
        }

        public async Task<RaceType> Insert(RaceType raceType)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceType = db.RaceTypes.Add(raceType);
                await db.SaveChangesAsync();
            }

            return raceType;
        }

        public async Task<EntityState> Update(int id, RaceType raceType)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(raceType).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.RaceTypes.Count(e => e.Id == id) == 0)
                    {
                        return EntityState.Unchanged;
                    }
                    else
                    {
                        throw;
                    }
                }

                return db.Entry(raceType).State;
            }
        }
    }
}