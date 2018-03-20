using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class BestLapTimeDTO
    {
        public string Id { get; set; }
        public string LapTimeId { get; set; }
        public string ApplicationUserId { get; set; }
        public string CarId { get; set; }
    }
}