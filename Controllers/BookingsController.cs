using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusReservation.Models;

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

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
