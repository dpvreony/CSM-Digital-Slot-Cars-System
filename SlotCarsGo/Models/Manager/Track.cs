using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo.Models.Manager
{
    class Track
    {
        string name;
        int id;
        float length;
        string macAddress;

        public Track(string name, int id, float length, string macAddress)
        {
            this.Name = name;
            this.Id = id;
            this.Length = length;
            this.MacAddress = macAddress;
        }

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public float Length { get => length; set => length = value; }
        public string MacAddress { get => macAddress; set => macAddress = value; }
    }
}
