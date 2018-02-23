using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SlotCarsGo_Server.Models
{
    /// <summary>
    /// Represents a temporary logged in user configured to participate in a race session.
    /// </summary>
    public class Driver
    {
        public int Id { get; set; }

        // Foreign Key
        public int? TrackId { get; set; }
        // Navigation property
        public Track Track { get; set; }

        public int ControllerId { get; set; }

        // Foreign Key
        public int? ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign key
        public int? CarId { get; set; }
        // Navigation property
        public Car Car { get; set; }
    }
}