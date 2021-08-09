using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class BusSeatNo
    {
        public int SeatId { get; set; }
        public int? BusId { get; set; }
        public int? SeatNo { get; set; }
        public bool? IsAvailable { get; set; }

        public virtual bus Bus { get; set; }
    }
}
