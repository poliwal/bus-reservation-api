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
    public class PassengerDetailsController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public PassengerDetailsController(BusReservationContext context)
        {
            _context = context;
        }

        // GET: api/PassengerDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerDetail>>> GetPassengerDetails()
        {
            return await _context.PassengerDetails.ToListAsync();
        }

        // GET: api/PassengerDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDetail>> GetPassengerDetail(int id)
        {
            var passengerDetail = await _context.PassengerDetails.FindAsync(id);

            if (passengerDetail == null)
            {
                return NotFound();
            }

            return passengerDetail;
        }

        // PUT: api/PassengerDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassengerDetail(int id, PassengerDetail passengerDetail)
        {
            if (id != passengerDetail.PassId)
            {
                return BadRequest();
            }

            _context.Entry(passengerDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerDetailExists(id))
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

        // POST: api/PassengerDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PassengerDetail>> PostPassengerDetail(List<PassengerDetail> passengerDetail)
        {
            foreach (var item in passengerDetail)
            {
                _context.PassengerDetails.Add(item);
            }

            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassengerDetail", new { id = passengerDetail }, passengerDetail);
        }

        // DELETE: api/PassengerDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassengerDetail(int id)
        {
            var passengerDetail = await _context.PassengerDetails.FindAsync(id);
            if (passengerDetail == null)
            {
                return NotFound();
            }

            _context.PassengerDetails.Remove(passengerDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassengerDetailExists(int id)
        {
            return _context.PassengerDetails.Any(e => e.PassId == id);
        }
    }
}
