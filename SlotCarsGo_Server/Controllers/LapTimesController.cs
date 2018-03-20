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
        [ResponseType(typeof(LapTimeDTO))]
        public async Task<IHttpActionResult> PostLapTime(IEnumerable<LapTimeDTO> lapTimeDTOs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TimeSpan fastestLap = lapTimeDTOs.Min(l => l.Time);
            BestLapTime bestLapTime = new BestLapTime();

            foreach (LapTimeDTO lapTimeDTO in lapTimeDTOs)
            {
                LapTime lapTime = Mapper.Map<LapTime>(lapTimeDTO);
                lapTime = await repo.Insert(lapTime);

                if (lapTimeDTO.Time == fastestLap)
                {
                    IRepositoryAsync<DriverResult> driverResultsRepo = new DriverResultsRepository<DriverResult>();
                    DriverResult driverResult = await driverResultsRepo.GetById(lapTime.DriverResultId);
                    bestLapTime = new BestLapTime()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ApplicationUserId = driverResult.ApplicationUserId,
                        CarId = driverResult.CarId,
                        LapTimeId = lapTime.Id,
                    };

                    BestLapTimesRepository<BestLapTime> bestLapsRepo = new BestLapTimesRepository<BestLapTime>();
                    bestLapTime = await bestLapsRepo.Insert(bestLapTime);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bestLapTime.Id }, Mapper.Map<BestLapTimeDTO>(bestLapTime));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}