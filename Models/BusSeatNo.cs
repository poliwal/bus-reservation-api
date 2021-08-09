using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class BusSeatNo
    {
        public BusSeatNo()
        {
            BookedSeats = new HashSet<BookedSeat>();
        }

        public int SeatId { get; set; }
        public int? BusId { get; set; }
        public int? SeatNo { get; set; }
        public bool? IsAvailable { get; set; }

        public virtual bus Bus { get; set; }
        public virtual ICollection<BookedSeat> BookedSeats { get; set; }
    }
}
