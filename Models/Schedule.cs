using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class Schedule
    {
        public int ScId { get; set; }
        public DateTime? ScDate { get; set; }
        public int? BusId { get; set; }

        public virtual bus Bus { get; set; }
    }
}
