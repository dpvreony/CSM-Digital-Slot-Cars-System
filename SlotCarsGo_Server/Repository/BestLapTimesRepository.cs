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
    public class BestLapTimesRepository<T> : IRepositoryAsync<BestLapTime> where T : BestLapTime
    {
        public async Task<BestLapTime> Delete(string id)
        {
            BestLapTime bestLapTime;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                bestLapTime = await db.BestLapTimes.FindAsync(id);
                if (bestLapTime != null)
                {
                    db.BestLapTimes.Remove(bestLapTime);
                    await db.SaveChangesAsync();
                }
            }

            return bestLapTime;
        }

        public bool Exists(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.BestLapTimes.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<BestLapTime> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.BestLapTimes;
            }
        }

        public async Task<BestLapTime> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.BestLapTimes.FindAsync(id);
            }
        }

        public IQueryable<BestLapTime> GetForUserId(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.BestLapTimes.Where(lt => lt.ApplicationUserId == userId);
            }
        }

        public async Task<BestLapTime> Insert(BestLapTime bestLapTime)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                bestLapTime.Id = string.IsNullOrEmpty(bestLapTime.Id) ? Guid.NewGuid().ToString() : bestLapTime.Id;
                bestLapTime = db.BestLapTimes.Add(bestLapTime); // register in context

                LapTime lapTime = await db.LapTimes
                    .Where(l => l.Id == bestLapTime.LapTimeId)
                    .Include(l => l.DriverResult)
                    .Include(l => l.DriverResult.ApplicationUser)
                    .Include(l => l.DriverResult.Car)
                    .Include(l => l.DriverResult.RaceSession)
                    .Include(l => l.DriverResult.RaceSession.Track)
                    .FirstOrDefaultAsync();

                TimeSpan time = bestLapTime.LapTime.Time;
                DriverResult driverResult = bestLapTime.LapTime.DriverResult;
                Car car = driverResult.Car;
                ApplicationUser user = driverResult.ApplicationUser;
                Track track = driverResult.RaceSession.Track;

                bool modified = false;

                // Check Track's overall record
                if (string.IsNullOrEmpty(track.BestLapTimeId)|| track.BestLapTime.LapTime.Time > time)
                {
                    track.BestLapTimeId = bestLapTime.Id;
                    modified = true;
                }

                // Check this car's overall record
                if (string.IsNullOrEmpty(car.BestLapTimeId) || car.BestLapTime.LapTime.Time > time)
                {
                    car.BestLapTimeId = bestLapTime.Id;
                    modified = true;
                }

                // Check this car's best time for this user
                BestLapTime usersCarBestLap = car.BestLapTimes.Where(l => l.ApplicationUserId == user.Id).FirstOrDefault();
                if (usersCarBestLap == null)
                {
                    modified = true;
                    user.BestLapTimes.Add(bestLapTime);
                    car.BestLapTimes.Add(bestLapTime);
                }
                else if (time < usersCarBestLap.LapTime.Time)
                {
                    usersCarBestLap.LapTimeId = bestLapTime.Id;
                    modified = true;
                }
                
                // If changes made then save best lap time
                if (modified)
                {
                    await db.SaveChangesAsync();
                }
            }

            return bestLapTime;
        }

        public async Task<EntityState> Update(string id, BestLapTime bestLapTime)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(bestLapTime).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.BestLapTimes.Count(e => e.Id == id) == 0)
                    {
                        return EntityState.Unchanged;
                    }
                    else
                    {
                        throw;
                    }
                }

                return db.Entry(bestLapTime).State;
            }
        }
    }
}