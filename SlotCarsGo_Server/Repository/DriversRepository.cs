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
    public class DriversRepository<T, DTO> : IRepositoryAsync<Driver, DriverDTO>
        where T : Driver
        where DTO : DriverDTO
    {
        public async Task<Driver> Delete(int id)
        {
            Driver car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.Drivers.FindAsync(id);
                if (car != null)
                {
                    db.Drivers.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Drivers.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<DriverDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Drivers.ProjectTo<DriverDTO>();
            }
        }

        public async Task<Driver> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Drivers.FindAsync(id);
            }
        }

        public IEnumerable<DriverDTO> GetForId(int trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Drivers.Where(d => d.Track.Id == trackId).ProjectTo<DriverDTO>();
            }
        }

        public async Task<Driver> Insert(Driver car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = db.Drivers.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(int id, Driver car)
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
                    if (db.Drivers.Count(e => e.Id == id) == 0)
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