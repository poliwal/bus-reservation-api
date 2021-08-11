using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class Booking
    {
        public Booking()
        {
            PassengerDetails = new HashSet<PassengerDetail>();
            ReturnBookings = new HashSet<ReturnBooking>();
        }

        public int BookingId { get; set; }
        public int? Cid { get; set; }
        public int? BusScId { get; set; }
        public int? ReturnBusId { get; set; }
        public int? NoOfPassengers { get; set; }
        public decimal? TotalFare { get; set; }
        public string Status { get; set; }
        public DateTime? DateOfBooking { get; set; }
        public bool? IsReturn { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? WholeBus { get; set; }
        public bool? WithDriver { get; set; }
        public decimal? SecurityDeposit { get; set; }

        public virtual BusSchedule BusSc { get; set; }
        public virtual Customer CidNavigation { get; set; }
        public virtual ICollection<PassengerDetail> PassengerDetails { get; set; }
        public virtual ICollection<ReturnBooking> ReturnBookings { get; set; }
    }
}
