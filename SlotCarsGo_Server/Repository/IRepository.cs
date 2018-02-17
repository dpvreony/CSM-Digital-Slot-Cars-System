using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo_Server.Repository
{
    public interface IRepository <T> where T:class
    {
        IQueryable<T> GetAll();
        T GetById(object Id);
        T Insert(T obj);
        void Delete(object Id);
        T Update(T obj);
        int Count();
        void Save();
    }
}
