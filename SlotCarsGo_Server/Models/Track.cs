using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    public class Track
    {
        public int Id { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }

        [Required]
        public string Name { get; set; }

        public float Length { get; set; }
    }
}