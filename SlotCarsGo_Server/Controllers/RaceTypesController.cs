using AutoMapper;
using AutoMapper.QueryableExtensions;
using SlotCarsGo_Server.Models;
using SlotCarsGo_Server.Models.DTO;
using SlotCarsGo_Server.Repository;
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

namespace SlotRaceTypesGo_Server.Controllers
{
    public class RaceTypesController : ApiController
    {
        private RaceTypesRepository<RaceType, RaceTypeDTO> repo = new RaceTypesRepository<RaceType, RaceTypeDTO>();

        // GET: api/RaceTypes/{TrackId}
        public IEnumerable<RaceTypeDTO> GetRaceTypes(string trackId)
        {
            return repo.GetAllAsDTO(trackId);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}