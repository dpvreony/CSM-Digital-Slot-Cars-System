using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class RaceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rules { get; set; }
        public string Symbol { get; set; }
    }
}