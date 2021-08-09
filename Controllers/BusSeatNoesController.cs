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
    public class BusSeatNoesController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public BusSeatNoesController(BusReservationContext context)
        {
            _context = context;
        }

        // GET: api/BusSeatNoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusSeatNo>>> GetBusSeatNos()
        {
            return await _context.BusSeatNos.ToListAsync();
        }

        // GET: api/BusSeatNoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusSeatNo>> GetBusSeatNo(int id)
        {
            var busSeatNo = await _context.BusSeatNos.FindAsync(id);

            if (busSeatNo == null)
            {
                return NotFound();
            }

            return busSeatNo;
        }

        // PUT: api/BusSeatNoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusSeatNo(int id, BusSeatNo busSeatNo)
        {
            if (id != busSeatNo.SeatId)
            {
                return BadRequest();
            }

            _context.Entry(busSeatNo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusSeatNoExists(id))
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

        // POST: api/BusSeatNoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusSeatNo>> PostBusSeatNo(BusSeatNo busSeatNo)
        {
            _context.BusSeatNos.Add(busSeatNo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusSeatNo", new { id = busSeatNo.SeatId }, busSeatNo);
        }

        // DELETE: api/BusSeatNoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusSeatNo(int id)
        {
            var busSeatNo = await _context.BusSeatNos.FindAsync(id);
            if (busSeatNo == null)
            {
                return NotFound();
            }

            _context.BusSeatNos.Remove(busSeatNo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusSeatNoExists(int id)
        {
            return _context.BusSeatNos.Any(e => e.SeatId == id);
        }
    }
}
