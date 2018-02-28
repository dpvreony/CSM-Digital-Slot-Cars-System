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
    public class TracksRepository<T, DTO> : IRepositoryAsync<Track, TrackDTO>
        where T : Track
        where DTO : TrackDTO
    {
        public async Task<Track> Delete(int id)
        {
            Track car;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = await db.Tracks.FindAsync(id);
                if (car != null)
                {
                    db.Tracks.Remove(car);
                    await db.SaveChangesAsync();
                }
            }

            return car;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks.Count(e => e.Id == id) > 0;
            }
        }

        public IEnumerable<TrackDTO> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks.ProjectTo<TrackDTO>();
            }
        }

        public async Task<Track> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Tracks.FindAsync(id);
            }
        }

        public IEnumerable<TrackDTO> GetForId(int trackId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks.Where(t => t.Id == trackId).ProjectTo<TrackDTO>();
            }
        }

        public async Task<Track> Insert(Track car)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                car = db.Tracks.Add(car);
                await db.SaveChangesAsync();
            }

            return car;
        }

        public async Task<EntityState> Update(int id, Track car)
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
                    if (db.Tracks.Count(e => e.Id == id) == 0)
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