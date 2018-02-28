using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo_Server.Repository
{
    public interface IRepositoryAsync <T, DTO> where T:class where DTO:class
    {
        Task<T> Delete(int id);
        bool Exists(int id);
        IEnumerable<DTO> GetAll();
        Task<T> GetById(int id);
        IEnumerable<DTO> GetForId(int trackId);
        Task<T> Insert(T obj);
        Task<EntityState> Update(int id, T obj);
    }
}
