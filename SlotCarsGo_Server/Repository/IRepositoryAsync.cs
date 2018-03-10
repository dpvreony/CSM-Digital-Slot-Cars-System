using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo_Server.Repository
{
    public interface IRepositoryAsync <T> where T:class
    {
        Task<T> Delete(string id);
        bool Exists(string id);
        IEnumerable<T> GetAll();
        Task<T> GetById(string id);
        IEnumerable<T> GetFor(string id);
        Task<T> Insert(T obj);
        Task<EntityState> Update(string id, T obj);
    }
}
