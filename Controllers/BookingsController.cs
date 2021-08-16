using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusReservation.Models;
using System.Net.Mail;
using System.Net;

namespace BusReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public BookingsController(BusReservationContext context)
        {
            _context = context;
        }

        #region Get all bookings
        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                return await _context.Bookings.ToListAsync();
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get booking by BookingId
        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }
        #endregion

        #region Get Booking by Cid
        [HttpGet]
        [Route("bookingForCid")]
        public IActionResult GetBookingForCid(int id)
        {
            try
            {
                var bookings = (from b in _context.Bookings
                                join bsc in _context.BusSchedules on b.BusScId equals bsc.BusScId
                                join bs in _context.Buses on bsc.BusNo equals bs.BusNo
                                where b.Cid == id
                                select new
                                {
                                    b.BookingId,
                                    b.Cid,
                                    b.BusScId,
                                    b.ReturnBusId,
                                    b.NoOfPassengers,
                                    b.TotalFare,
                                    b.Status,
                                    b.DateOfBooking,
                                    b.IsReturn,
                                    b.ReturnDate,
                                    b.WholeBus,
                                    b.WithDriver,
                                    b.SecurityDeposit,
                                    bsc.DepartureDate,
                                    bsc.AvailableSeats,
                                    bs.BusNo,
                                    bs.BusName,
                                    bs.Source,
                                    bs.Destination,
                                    bs.DepartureTime,
                                    bs.ArrivalTime,
                                    bs.NoOfSeats,
                                    bs.Via,
                                    bs.Fare,
                                    bs.DriverName,
                                    bs.DriverAge,
                                    bs.DriverExperience
                                }).ToList();

                if (bookings == null)
                {
                    return NotFound();
                }

                return Ok(bookings);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Update Booking
        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region Add Booking
        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            try
            {
                var cust = _context.Customers.Where(c => c.Cid == booking.Cid).FirstOrDefault();
                if(cust != null)
                {
                    if (cust.IsAuthorized == true)
                    {
                        cust.HasBooked = true;
                    }
                }
                
                
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete Booking
        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Send E-Ticket SMTP
        [HttpGet]
        [Route("sendTicket")]
        public IActionResult SendTicket([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "bookingId")] int BookingId,
            [FromQuery(Name = "busNo")] int BusNo, [FromQuery(Name = "source")] string Source, [FromQuery(Name = "dest")] string Destination,
            [FromQuery(Name = "date")] string Date)
        {
            var cust = _context.Customers.Where(c => c.Cid == Cid).FirstOrDefault();
            if (cust == null)
            {
                return BadRequest("invalid email");
            }

            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("ltibusreservation@gmail.com", ""),
            };

            string subject = "Bus Reservation E-Ticket";

            string body = "Ticket No: "+BookingId+"\nBus No: "+BusNo+"\nSource: "+Source+"\nDestination: "+Destination+"\nDeparture Date: "+Date;
            if (cust.IsAuthorized == false)
            {
                body = body + "\nNote: Register with us to cancel or to view more details of your ticket.";
            }
            try
            {
                email.Send("ltibusreservation@gmail.com", cust.Email, subject, body);
                return Ok("Email sent");
            }
            catch (Exception e)
            {
                return BadRequest("Email not sent");
            }
        }
        #endregion

        #region Get Last Month Records and Profits
        [HttpGet]
        [Route("lastMonthRecordAndProfits")]
        public IActionResult LastMonthRecordAndProfits(string lastMonthDate, string currentDate)
        {
            try
            {
                var record = _context.Bookings.Where(x => x.DateOfBooking >= Convert.ToDateTime(lastMonthDate) &&
             x.DateOfBooking < Convert.ToDateTime(currentDate)).ToList();

                return Ok(record);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        #endregion

        #region Get Customer Reservation Details
        #region Get today's Reservation Details 
        [HttpGet]
        [Route("getCustomerReservationDetailsOfToday")]
        public IActionResult GetCustomerReservationDetails(string currentDate)
        {
            try
            {
                var res = (from bk in _context.Bookings
                           join c in _context.Customers
                           on bk.Cid equals c.Cid
                           where bk.DateOfBooking == Convert.ToDateTime(currentDate)
                           select bk).ToList();

                return Ok(res);
            }
            catch (Exception e)
            {

                return NotFound(e.Message);
            }
        }
        #endregion

        #region Get Weekly Reservation Details 
        [HttpGet]
        [Route("getCustomerReservationDetailsOfWeek")]
        public IActionResult GetCustomerReservationDetailsOfWeek(string weekDate, string currentDate)
        {
            try
            {
                var res = (from bk in _context.Bookings
                           join c in _context.Customers
                           on bk.Cid equals c.Cid
                           where bk.DateOfBooking >= Convert.ToDateTime(weekDate) && bk.DateOfBooking <= Convert.ToDateTime(currentDate)
                           select bk).ToList();

                return Ok(res);
            }
            catch (Exception e)
            {

                return NotFound(e.Message);
            }
        }
        #endregion

        #region Get Monthly Reservation Details 
        [HttpGet]
        [Route("getCustomerReservationDetailsOfMonth")]
        public IActionResult GetCustomerReservationDetailsOfMonth(string monthDate, string currentDate)
        {

            try
            {
                var res = (from bk in _context.Bookings
                           join c in _context.Customers
                           on bk.Cid equals c.Cid
                           where bk.DateOfBooking >= Convert.ToDateTime(monthDate) && bk.DateOfBooking <= Convert.ToDateTime(currentDate)
                           select bk).ToList();

                return Ok(res);
            }
            catch (Exception e)
            {

                return NotFound(e.Message);
            }
        }
        #endregion
        #endregion

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
