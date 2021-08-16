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
    public class busesController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public busesController(BusReservationContext context)
        {
            _context = context;
        }

        #region Get All Buses
        // GET: api/buses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<bus>>> GetBuses()
        {
            try
            {
                return await _context.Buses.ToListAsync();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get Bus by BusNo
        // GET: api/buses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bus>> Getbus(int id)
        {
            try
            {
                var bus = await _context.Buses.FindAsync(id);

                if (bus == null)
                {
                    return NotFound();
                }

                return bus;
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        #region Get Frequently Travelled Routes
        [HttpGet]
        [Route("getFrequentlyTravelledRoutes")]
        public IActionResult GetFrequentlyTravelledRoutes()
        {
            try
            {
                var route = (from b in _context.Buses
                             select new
                             {
                                 b.Via,
                                 frequency = b.Via.Count()
                             }).ToList();

                return Ok(route);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        #endregion

        #region Update Bus
        // PUT: api/buses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putbus(int id, bus bus)
        {
            if (id != bus.BusNo)
            {
                return BadRequest();
            }

            _context.Entry(bus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!busExists(id))
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


        #region Add Bus
        // POST: api/buses
        [HttpPost]
        public async Task<ActionResult<bus>> Postbus(bus bus)
        {
            try
            {
                _context.Buses.Add(bus);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Getbus", new { id = bus.BusNo }, bus);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete Bus
        // DELETE: api/buses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletebus(int id)
        {
            try
            {
                var bus = await _context.Buses.FindAsync(id);
                if (bus == null)
                {
                    return NotFound();
                }

                _context.Buses.Remove(bus);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion  

        private bool busExists(int id)
        {
            return _context.Buses.Any(e => e.BusNo == id);
        }
    }
}
