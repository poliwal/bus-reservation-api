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

        // GET: api/ReturnBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnBooking>>> GetReturnBookings()
        {
            return await _context.ReturnBookings.ToListAsync();
        }

        // GET: api/ReturnBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnBooking>> GetReturnBooking(int id)
        {
            var returnBooking = await _context.ReturnBookings.FindAsync(id);

            if (returnBooking == null)
            {
                return NotFound();
            }

            return returnBooking;
        }

        // PUT: api/ReturnBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/ReturnBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReturnBooking>> PostReturnBooking(ReturnBooking returnBooking)
        {
            _context.ReturnBookings.Add(returnBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReturnBooking", new { id = returnBooking.ReturnBookingId }, returnBooking);
        }

        // DELETE: api/ReturnBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturnBooking(int id)
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

        private bool ReturnBookingExists(int id)
        {
            return _context.ReturnBookings.Any(e => e.ReturnBookingId == id);
        }
    }
}
