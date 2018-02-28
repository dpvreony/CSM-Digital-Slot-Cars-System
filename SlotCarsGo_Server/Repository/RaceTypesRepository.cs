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
    public class RaceTypesRepository<T, DTO> : IRepositoryAsync<RaceType, RaceTypeDTO>
        where T : RaceType
        where DTO : RaceTypeDTO
    {
        public async Task<RaceType> Delete(int id)
        {
            RaceType car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.RaceTypes.FindAsync(id);
                if (car != null)
                {
                    db.RaceTypes.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<RaceTypeDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.ProjectTo<RaceTypeDTO>();
            }
        }

        public async Task<RaceType> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceTypes.FindAsync(id);
            }
        }

        public IEnumerable<RaceTypeDTO> GetForId(int trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.ProjectTo<RaceTypeDTO>();
            }
        }

        public async Task<RaceType> Insert(RaceType car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = db.RaceTypes.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(int id, RaceType car)
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
                    if (db.RaceTypes.Count(e => e.Id == id) == 0)
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