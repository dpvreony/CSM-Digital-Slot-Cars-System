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
        Task<T> Delete(string id);
        bool Exists(string id);
        IEnumerable<DTO> GetAll();
        Task<T> GetById(string id);
        IEnumerable<DTO> GetForId(string id);
        Task<T> Insert(T obj);
        Task<EntityState> Update(string id, T obj);
    }
}
