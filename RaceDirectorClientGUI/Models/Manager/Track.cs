using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDirectorClientGUI.Models.Manager
{
    class Track
    {
        string name;
        int id;
        float length;
        string macAddress;
        User owner;

        public Track(string name, int id)
        {
            this.name = name;
            this.id = id;
        }

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public float Length { get => length; set => length = value; }
        public string MacAddress { get => macAddress; set => macAddress = value; }
        internal User Owner { get => owner; set => owner = value; }
    }
}
