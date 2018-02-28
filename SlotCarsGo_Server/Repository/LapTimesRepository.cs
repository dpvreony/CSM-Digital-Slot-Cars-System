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
    public class LapTimesRepository<T, DTO> : IRepositoryAsync<LapTime, LapTimeDTO>
        where T : LapTime
        where DTO : LapTimeDTO
    {
        public async Task<LapTime> Delete(int id)
        {
            LapTime car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.LapTimes.FindAsync(id);
                if (car != null)
                {
                    db.LapTimes.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<LapTimeDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.LapTimes.ProjectTo<LapTimeDTO>();
            }
        }

        public async Task<LapTime> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.LapTimes.FindAsync(id);
            }
        }

        public IEnumerable<LapTimeDTO> GetForId(int driverResultId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                DriverResult dr = db.DriverResults.Find(driverResultId);

                return db.LapTimes.Where(lt => lt.RaceSessionId == dr.SessionId && lt.DriverId == dr.ApplicationUserId).ProjectTo<LapTimeDTO>();
            }
        }

        public async Task<LapTime> Insert(LapTime car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = db.LapTimes.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(int id, LapTime car)
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
                    if (db.LapTimes.Count(e => e.Id == id) == 0)
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