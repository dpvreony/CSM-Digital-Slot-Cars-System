using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo.Models.Manager
{
    public class Track
    {
        public Track()
        {
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public float Length { get; set; }
        public string Secret { get; set; }
        public string MacAddress { get; set; }
    }
}
