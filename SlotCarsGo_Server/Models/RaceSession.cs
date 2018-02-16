using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotCarsGo_Server.Models
{
    /// <summary>
    /// Session entity for storing in DB with Entity Framework.
    /// </summary>
    public class RaceSession
    {
        public int Id { get; set; }

        // Foreign key
        [Required]
        public int TrackID { get; set; }
        // Navigation property
        public Track Track { get; set; }

        // Foreign key
        [Required]
        public int RaceTypeId { get; set; }
        // Navigation property
        public RaceType RaceType;

        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public bool FuelEnabled { get; set; }
        [Required]
        public int NumberOfDrivers { get; set; }
        [Required]
        public int RaceLimitValue { get; set; }
        [Required]
        public TimeSpan RaceLength { get; set; }
        [Required]
        public bool LapsNotDuration { get; set; }
        [Required]
        public int CrashPenalty { get; set; }
    }
}
