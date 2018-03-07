using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Track
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        public string Name { get; set; }

        public float Length { get; set; }

        public string Secret { get; set; }
    }
}