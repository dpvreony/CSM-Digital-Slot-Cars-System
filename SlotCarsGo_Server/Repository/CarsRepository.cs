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
    public class CarsRepository<T> : IRepositoryAsync<Car> where T : Car 
    {
        public async Task<Car> Delete(string id)
        {
            Car car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.Cars.FindAsync(id);
                if (car != null)
                {
                    db.Cars.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cars.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<Car> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cars;
            }
        }

        public async Task<Car> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Cars.FindAsync(id);
            }
        }

        public IQueryable<Car> GetForId(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cars.Where(c => c.TrackId == trackId);
            }
        }

        public async Task<Car> Insert(Car car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car.Id = car.Id == null | car.Id == string.Empty ? Guid.NewGuid().ToString() : car.Id;
                car = db.Cars.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(string id, Car car)
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
                    if (db.Cars.Count(e => e.Id == id) == 0)
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