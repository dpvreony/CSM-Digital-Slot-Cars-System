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
        Task<T> Delete(int id);
        bool Exists(int id);
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        Task<T> Insert(T obj);
        Task<EntityState> Update(int id, T obj);
    }
}
