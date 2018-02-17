using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Repository
{
    public class TracksRepository<Track> : IRepository<Track> where Track : class
    {
        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(object Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Track> GetAll()
        {
            throw new NotImplementedException();
        }

        public Track GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public Track Insert(Track track)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Track Update(Track track)
        {
            throw new NotImplementedException();
        }
    }
}