using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class BusSchedule
    {
        public BusSchedule()
        {
            Bookings = new HashSet<Booking>();
            BusSeatNos = new HashSet<BusSeatNo>();
            ReturnBookings = new HashSet<ReturnBooking>();
        }

        public int BusScId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int? BusNo { get; set; }

        public virtual bus BusNoNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BusSeatNo> BusSeatNos { get; set; }
        public virtual ICollection<ReturnBooking> ReturnBookings { get; set; }
    }
}
