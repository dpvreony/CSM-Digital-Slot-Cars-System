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
    public class RaceTypesRepository<T, DTO> : IRepositoryAsync<RaceType>, IRepositoryDTOAsync<RaceTypeDTO>
    {
        public async Task<RaceType> Delete(string id)
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

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<RaceType> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes;
            }
        }

        public IEnumerable<RaceTypeDTO> GetAllAsDTO(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes.ProjectTo<RaceTypeDTO>();
            }
        }

        public async Task<RaceType> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.RaceTypes.FindAsync(id);
            }
        }

        public IEnumerable<RaceType> GetForId(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.RaceTypes;
            }
        }

        public async Task<RaceType> Insert(RaceType raceType)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                raceType.Id = raceType.Id == null | raceType.Id == string.Empty ? Guid.NewGuid().ToString() : raceType.Id;
                raceType = db.RaceTypes.Add(raceType);
                await db.SaveChangesAsync();
            }

            return raceType;
        }

        public async Task<EntityState> Update(string id, RaceType raceType)
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