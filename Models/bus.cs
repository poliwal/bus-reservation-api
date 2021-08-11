using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class bus
    {
        public bus()
        {
            BusSchedules = new HashSet<BusSchedule>();
        }

        public int BusNo { get; set; }
        public string BusName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public int? NoOfSeats { get; set; }
        public string Via { get; set; }
        public decimal? Fare { get; set; }
        public string DriverName { get; set; }
        public int? DriverAge { get; set; }
        public int? DriverExperience { get; set; }

        public virtual ICollection<BusSchedule> BusSchedules { get; set; }
    }
}
