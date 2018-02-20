﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models.DTO;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace SlotCarsGo_Server.Controllers
{
    public class CarsController : ApiController
    {
        private IRepositoryAsync<Car> repo = new CarsRepository<Car>();

        // GET: api/Cars
        public IQueryable<CarDTO> GetCars()
        {
            return repo.GetAll().ProjectTo<CarDTO>();
        }

        // GET: api/Cars/5
        [ResponseType(typeof(CarDTO))]
        public async Task<IHttpActionResult> GetCar(int id)
        {
            Car car = await repo.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CarDTO>(car));
        }

        // PUT: api/Cars/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCar(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != car.Id)
            {
                return BadRequest();
            }

            if (await repo.Update(id, car) != EntityState.Modified)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cars
        [ResponseType(typeof(CarDTO))]
        public async Task<IHttpActionResult> PostCar(Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            car = await repo.Insert(car);

            return CreatedAtRoute("DefaultApi", new { id = car.Id }, Mapper.Map<CarDTO>(car));
        }

        // DELETE: api/Cars/5
        [ResponseType(typeof(CarDTO))]
        public async Task<IHttpActionResult> DeleteCar(int id)
        {
            Car car = await repo.Delete(id);
            if (car == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CarDTO>(car));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}