using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class LapTimesController : ApiController
    {
        private IRepositoryAsync<LapTime> repo = new LapTimesRepository<LapTime>();

        // POST: api/LapTimes
//        [ResponseType(typeof(LapTimeDTO))]
        [Route("api/LapTimes")]
        public async Task<HttpResponseMessage> PostLapTime(LapTimeDTO[] lapTimeDTOs)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            LapTimeDTO first = lapTimeDTOs.FirstOrDefault();
            if (first != null)
            {
                DriverResultsRepository<DriverResult> driverResultsRepo = new DriverResultsRepository<DriverResult>();
                CarsRepository<Car, CarDTO> carsRepo = new CarsRepository<Car, CarDTO>();
                TracksRepository<Track> tracksRepo = new TracksRepository<Track>();
                BestLapTimesRepository<BestLapTime> bestLapsRepo = new BestLapTimesRepository<BestLapTime>();

                DriverResult driverResult = await driverResultsRepo.GetById(first.DriverResultId);

                BestLapTime usersBestLapInCar = bestLapsRepo.GetForUserId(driverResult.ApplicationUserId).Where(bl => bl.CarId == driverResult.CarId).FirstOrDefault();
                TimeSpan fastestLap = driverResult.BestLapTime;
                BestLapTime bestLapTime = new BestLapTime();

                foreach (LapTimeDTO lapTimeDTO in lapTimeDTOs)
                {
                    LapTime lapTime = new LapTime()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DriverResultId = lapTimeDTO.DriverResultId,
                        LapNumber = lapTimeDTO.LapNumber,
                        Time = lapTimeDTO.Time
                    };

                    lapTime = await repo.Insert(lapTime);

                    if (lapTime.Time == fastestLap)
                    {
                        bestLapTime = new BestLapTime()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ApplicationUserId = driverResult.ApplicationUserId,
                            CarId = driverResult.CarId,
                            LapTimeId = lapTime.Id,
                        };

                        if (usersBestLapInCar == null)
                        {
                            bestLapTime = await bestLapsRepo.Insert(bestLapTime);
                        }
                        else if (fastestLap < usersBestLapInCar.LapTime.Time)
                        {
                            EntityState response = await bestLapsRepo.Update(usersBestLapInCar.Id, bestLapTime);
                        }

                        Car car = await carsRepo.GetById(driverResult.CarId);
                        var carsBestLapTime = car?.BestLapTime?.LapTime?.Time;
                        if (car?.BestLapTimeId == null || fastestLap < carsBestLapTime)
                        {
                            car.BestLapTimeId = bestLapTime.Id;
                            await carsRepo.Update(car.Id, car);
                        }

                        Track track = await tracksRepo.GetById(car?.TrackId);
                        var trackBestLap = track?.BestLapTime?.LapTime?.Time;
                        if (track?.BestLapTimeId == null || fastestLap < trackBestLap)
                        {
                            track.BestLapTimeId = bestLapTime.Id;
                            await tracksRepo.Update(track.Id, track);
                        }
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}