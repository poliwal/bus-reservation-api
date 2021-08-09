using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class BookedSeat
    {
        public int BookedSeatId { get; set; }
        public int? BookingId { get; set; }
        public int? SeatId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual BusSeatNo Seat { get; set; }
    }
}
