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
    public class ReturnBookingsController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public ReturnBookingsController(BusReservationContext context)
        {
            _context = context;
        }

        #region Get all return Bookings
        // GET: api/ReturnBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnBooking>>> GetReturnBookings()
        {
            try
            {
                return await _context.ReturnBookings.ToListAsync();
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get return booking by ReturnBookingId
        // GET: api/ReturnBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnBooking>> GetReturnBooking(int id)
        {
            try
            {
                var returnBooking = await _context.ReturnBookings.FindAsync(id);

                if (returnBooking == null)
                {
                    return NotFound();
                }

                return returnBooking;
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get return booking by BookingId
        // GET: api/ReturnBookings/5
        [HttpGet]
        [Route("byBookingId")]
        public IActionResult GetReturnBookingByBookingId(int bookingId)
        {
            try
            {
                var returnBooking = _context.ReturnBookings.Where(rb => rb.BookingId == bookingId).Select(rb => rb.BusScId).FirstOrDefault();

                if (returnBooking == null)
                {
                    return NotFound();
                }

                return Ok(returnBooking);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }

        }
        #endregion

        #region Update a Return Booking
        // PUT: api/ReturnBookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReturnBooking(int id, ReturnBooking returnBooking)
        {
            if (id != returnBooking.ReturnBookingId)
            {
                return BadRequest();
            }

            _context.Entry(returnBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnBookingExists(id))
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

        #region Add Return Booking
        // POST: api/ReturnBookings
        [HttpPost]
        public async Task<ActionResult<ReturnBooking>> PostReturnBooking(ReturnBooking returnBooking)
        {
            try
            {
                _context.ReturnBookings.Add(returnBooking);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReturnBooking", new { id = returnBooking.ReturnBookingId }, returnBooking);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete a Return Booking
        // DELETE: api/ReturnBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturnBooking(int id)
        {
            try
            {
                var returnBooking = await _context.ReturnBookings.FindAsync(id);
                if (returnBooking == null)
                {
                    return NotFound();
                }

                _context.ReturnBookings.Remove(returnBooking);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        #endregion

        private bool ReturnBookingExists(int id)
        {
            return _context.ReturnBookings.Any(e => e.ReturnBookingId == id);
        }
    }
}
