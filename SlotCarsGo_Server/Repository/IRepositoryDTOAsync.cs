using SlotCarsGo_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SlotCarsGo_Server.Repository
{
    public interface IRepositoryDTOAsync<DTO> where DTO:class
    {
        IEnumerable<DTO> GetAllAsDTO();
    }
}