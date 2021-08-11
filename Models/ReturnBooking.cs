using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class ReturnBooking
    {
        public int ReturnBookingId { get; set; }
        public int? BookingId { get; set; }
        public int? BusScId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual BusSchedule BusSc { get; set; }
    }
}
