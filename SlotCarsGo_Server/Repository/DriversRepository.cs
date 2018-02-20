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
    public class DriversRepository<T> : IRepositoryAsync<Driver> where T : Driver
    {
        public async Task<Driver> Delete(int id)
        {
            Driver driver;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                driver = await db.Drivers.FindAsync(id);
                if (driver != null)
                {
                    db.Drivers.Remove(driver);
                    await db.SaveChangesAsync();
                }
            }

            return driver;
        }

        public bool Exists(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Drivers.Count(e => e.Id == id) > 0;
            }
        }

        public IQueryable<Driver> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Drivers;
            }
        }

        public async Task<Driver> GetById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Drivers.FindAsync(id);
            }
        }

        public async Task<Driver> Insert(Driver driver)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                driver = db.Drivers.Add(driver);
                await db.SaveChangesAsync();
            }

            return driver;
        }

        public async Task<EntityState> Update(int id, Driver driver)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Entry(driver).State = EntityState.Modified;

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

                return db.Entry(driver).State;
            }
        }
    }
}