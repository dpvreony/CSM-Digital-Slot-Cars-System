using System;
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
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace SlotCarsGo_Server.Controllers
{
    public class CarsController : ApiController
    {
        private CarsRepository<Car, CarDTO> repo = new CarsRepository<Car, CarDTO>();

        // GET: api/Cars/{trackId}
        [Route("api/cars/{trackId}")]
        public IEnumerable<CarDTO> GetCars(string trackId)
        {
            IEnumerable<CarDTO> cars = repo.GetAllAsDTO(trackId);
            string carImagesFolderPath = HttpContext.Current.Server.MapPath("~/Content/Images/Cars");
            foreach (CarDTO car in cars)
            {
                string carImagePath = Path.Combine(carImagesFolderPath, car.ImageName);
                if (File.Exists(carImagePath))
                {
                    if (File.Exists(carImagePath))
                    {
                        Image image = new Bitmap(carImagePath);
                        int height = 200;
                        int width = 200;
                        if (image.Height > image.Width)
                        {
                            float scale = image.Height / height;
                            width = Convert.ToInt32(image.Width / scale);
                        }
                        else if (image.Width > image.Height)
                        {
                            float scale = image.Width / width;
                            height = Convert.ToInt32(image.Height / scale);
                        }

                        image = new Bitmap(image, width, height);

                        MemoryStream ms = new MemoryStream();
                        switch (Path.GetExtension(carImagePath))
                        {
                            case ".jpeg":
                                image.Save(ms, ImageFormat.Jpeg);
                                break;
                            case ".jpg":
                                image.Save(ms, ImageFormat.Jpeg);
                                break;
                            case ".png":
                                image.Save(ms, ImageFormat.Png);
                                break;
                            case ".bmp":
                                image.Save(ms, ImageFormat.Bmp);
                                break;
                            case ".gif":
                                image.Save(ms, ImageFormat.Gif);
                                break;
                            default:
                                break;
                        }

                        car.CarImageBytes = ms.ToArray();
                    }
                }
            }

            return cars;
        }

        // GET: api/Cars/5
        [ResponseType(typeof(CarDTO))]
        public async Task<IHttpActionResult> GetCar(string id)
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
        public async Task<IHttpActionResult> PutCar(string id, Car car)
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
        public async Task<IHttpActionResult> DeleteCar(string id)
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