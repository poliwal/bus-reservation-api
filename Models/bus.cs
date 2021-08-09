using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class bus
    {
        public bus()
        {
            Bookings = new HashSet<Booking>();
            BusSeatNos = new HashSet<BusSeatNo>();
            Schedules = new HashSet<Schedule>();
        }

        public int BusId { get; set; }
        public string BusName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public TimeSpan? Departure { get; set; }
        public TimeSpan? Arrival { get; set; }
        public int? SeatsAvailable { get; set; }
        public string Via { get; set; }
        public decimal? Fare { get; set; }
        public string DriverName { get; set; }
        public int? DriverAge { get; set; }
        public int? DriverExperience { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BusSeatNo> BusSeatNos { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
