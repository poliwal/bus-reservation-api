using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class PassengerDetail
    {
        public int PassId { get; set; }
        public string Pname { get; set; }
        public int? Page { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
