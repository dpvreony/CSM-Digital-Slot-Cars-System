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
        public async Task<Track> Delete(string id)
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

        public bool Exists(string id)
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

        public async Task<Track> GetById(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Tracks.FindAsync(id);
            }
        }

        public IEnumerable<TrackDTO> GetForId(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks.Where(t => t.ApplicationUserId == userId).ProjectTo<TrackDTO>();
            }
        }

        public async Task<Track> Insert(Track track)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                track = db.Tracks.Add(track);
                await db.SaveChangesAsync();
                track.Secret = track.Id.Substring(0, 5);
                await db.SaveChangesAsync();
            }

            return track;
        }

        public async Task<EntityState> Update(string id, Track track)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(track).State = EntityState.Modified;

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

                return db.Entry(track).State;
            }
        }
    }
}