using AutoMapper;
using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SlotCarsGo_Server.Repository
{
    public class CarsRepository<T, DTO> : IRepositoryAsync<Car> , IRepositoryDTOAsync<CarDTO>
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

        public IEnumerable<Car> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cars;
            }
        }

        public IEnumerable<CarDTO> GetAllAsDTO(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<CarDTO> carDTOs = new List<CarDTO>();
                IQueryable<Car> cars = db.Cars
                    .Where(d => d.TrackId == trackId)
                    .Include(d => d.BestLapTime)
                    .Include(d => d.BestLapTime.LapTime)
                    .Include(d => d.BestLapTime.ApplicationUser)
                    .OrderBy(d => d.Name);

                foreach (Car car in cars)
                {
                    carDTOs.Add(Mapper.Map<CarDTO>(car));
                }

                return carDTOs;
            }
        }

        public async Task<Car> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Cars.Where(c => c.Id == id).Include(c => c.BestLapTime).Include(c => c.BestLapTimes).FirstOrDefaultAsync();
            }
        }

        public IEnumerable<Car> GetFor(string trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
/*
                return db.Cars.Where(c => c.TrackId == trackId)
                    .Include(c => c.BestLapTime)
                    .Include(c => c.BestLapTime.ApplicationUser)
                    .Include(c => c.BestLapTimes)
                    .Include(c => c.BestLapTimes)
                    .OrderBy(c => c.Name).ToList();
*/
                return db.Cars.Where(c => c.TrackId == trackId && c.BestLapTimeId != null)
                    .Include(c => c.BestLapTime)
                    .Include(c => c.BestLapTime.ApplicationUser)
                    .Include(c => c.BestLapTime.LapTime)
                    .OrderBy(c => c.BestLapTime.LapTime.Time)
                    .Concat(
                        db.Cars.Where(c => c.TrackId == trackId && c.BestLapTimeId == null).OrderBy(c => c.Name))
                    .ToList();
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

        /// <summary>
        /// Removes car from garage so that car can't be selected for new races.
        /// </summary>
        /// <param name="id">Car id.</param>
        /// <returns>the car.</returns>
        public async Task<Car> Remove(string id)
        {
            Car car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.Cars.FindAsync(id);
                if (car != null)
                {
                    car.Selectable = false;
                    await db.SaveChangesAsync();
                }
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