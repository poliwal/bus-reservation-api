using System;
using System.Collections.Generic;

#nullable disable

namespace BusReservation.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Cid { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public decimal? Wallet { get; set; }
        public int? Feedback { get; set; }
        public bool? HasBooked { get; set; }
        public bool? IsAuthorized { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
