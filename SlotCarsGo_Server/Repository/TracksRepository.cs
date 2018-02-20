﻿using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SlotCarsGo_Server.Repository
{
    public class TracksRepository<T> : IRepositoryAsync<Track> where T : Track
    {
        public async Task<Track> Delete(int id)
        {
            Track track;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                track = await db.Tracks.FindAsync(id);
                if (track != null)
                {
                    db.Tracks.Remove(track);
                    await db.SaveChangesAsync();
                }
            }

            return track;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<Track> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tracks;
            }
        }

        public async Task<Track> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Tracks.FindAsync(id);
            }
        }

        public async Task<Track> Insert(Track track)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                track = db.Tracks.Add(track);
                await db.SaveChangesAsync();
            }

            return track;
        }

        public async Task<EntityState> Update(int id, Track track)
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