using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookedSeats = new HashSet<BookedSeat>();
            PassengerDetails = new HashSet<PassengerDetail>();
        }

        public int BookingId { get; set; }
        public int? Cid { get; set; }
        public int? BusId { get; set; }
        public int? NoOfPassengers { get; set; }
        public decimal? TotalFare { get; set; }
        public string Status { get; set; }
        public DateTime? DateOfBooking { get; set; }
        public bool? IsReturn { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool? WholeBus { get; set; }
        public bool? WithDriver { get; set; }
        public decimal? SecurityDeposit { get; set; }

        public virtual bus Bus { get; set; }
        public virtual Customer CidNavigation { get; set; }
        public virtual ICollection<BookedSeat> BookedSeats { get; set; }
        public virtual ICollection<PassengerDetail> PassengerDetails { get; set; }
    }
}
