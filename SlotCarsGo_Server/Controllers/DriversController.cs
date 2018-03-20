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
using AutoMapper.QueryableExtensions;
using AutoMapper;
using SlotCarsGo_Server.Repository;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace SlotDriversGo_Server.Controllers
{
    public class DriversController : ApiController
    {
        private DriversRepository<Driver, DriverDTO> repo = new DriversRepository<Driver, DriverDTO>();

        // GET: api/Drivers/5
        [Route("api/Drivers/{trackId}")]
        public IEnumerable<DriverDTO> GetDrivers(string trackId)
        {
            IEnumerable<DriverDTO> drivers = repo.GetAllAsDTO(trackId);
            string carImagesFolderPath = HttpContext.Current.Server.MapPath("~/Content/Images/Cars");
            string userImagesFolderPath = HttpContext.Current.Server.MapPath("~/Content/Images/Users");
            foreach (DriverDTO driver in drivers)
            {
                string userImagePath = Path.Combine(userImagesFolderPath, driver.ImageName);
                if (File.Exists(userImagePath))
                {
                    Image image = new Bitmap(userImagePath);
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
                        height = Convert.ToInt32(image.Height/ scale);
                    }

                    image = new Bitmap(image, width, height);

                    MemoryStream ms = new MemoryStream();
                    switch (Path.GetExtension(userImagePath))
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

                    driver.UserImageBytes = ms.ToArray();
                }

                string carImagePath = Path.Combine(carImagesFolderPath, driver.SelectedCar.ImageName);
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

                        driver.SelectedCar.CarImageBytes = ms.ToArray();
                    }
                }
            }

            return drivers;
        }

        // DELETE: api/Drivers/5
        [ResponseType(typeof(DriverDTO))]
        public async Task<IHttpActionResult> DeleteDriver(string userId)
        {
            DriversRepository<Driver, DriverDTO> driversRepo = new DriversRepository<Driver, DriverDTO>();
            Driver driver = driversRepo.GetForUser(userId);
            if (driver == null)
            {
                return NotFound();
            }

            await driversRepo.Delete(driver.Id);

            return Ok(Mapper.Map<DriverDTO>(driver));
        }





        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}