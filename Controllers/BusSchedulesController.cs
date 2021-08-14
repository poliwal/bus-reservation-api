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
    public class BusSchedulesController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public BusSchedulesController(BusReservationContext context)
        {
            _context = context;
        }

        #region Get all Bus Schedules
        // GET: api/BusSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusSchedule>>> GetBusSchedules()
        {
            try
            {
                return await _context.BusSchedules.ToListAsync();
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Search Buses scheduled
        [HttpGet]
        [Route("searchBuses")]
        public IActionResult SearchBuses([FromQuery(Name = "source")] string Source,
            [FromQuery(Name = "destination")] string Destination, [FromQuery(Name = "date")] string Date)
        {
            try
            {
                var res = (from bs in _context.BusSchedules
                           join b in _context.Buses
                           on bs.BusNo equals b.BusNo
                           where b.Source == Source && b.Destination == Destination &&
                           bs.DepartureDate == Convert.ToDateTime(Date)
                           select new
                           {
                               bs.BusScId,
                               bs.DepartureDate,
                               b.BusNo,
                               b.BusName,
                               b.Source,
                               b.Destination,
                               b.DepartureTime,
                               b.ArrivalTime,
                               b.NoOfSeats,
                               b.Via,
                               b.Fare,
                               b.DriverName,
                               b.DriverAge,
                               b.DriverExperience
                           }).ToList();

                return Ok(res);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get Bus Schedule by BusScheduleId
        // GET: api/BusSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusSchedule>> GetBusSchedule(int id)
        {
            try
            {
                var busSchedule = await _context.BusSchedules.FindAsync(id);

                if (busSchedule == null)
                {
                    return NotFound();
                }

                return busSchedule;
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion


        #region Update Bus Schedule
        // PUT: api/BusSchedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusSchedule(int id, BusSchedule busSchedule)
        {
            if (id != busSchedule.BusScId)
            {
                return BadRequest();
            }

            _context.Entry(busSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusScheduleExists(id))
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


        #region Add Bus Schedule
        // POST: api/BusSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusSchedule>> PostBusSchedule(BusSchedule busSchedule)
        {
            try
            {
                _context.BusSchedules.Add(busSchedule);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBusSchedule", new { id = busSchedule.BusScId }, busSchedule);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion


        #region Delete Bus Schedule
        // DELETE: api/BusSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusSchedule(int id)
        {
            try
            {
                var busSchedule = await _context.BusSchedules.FindAsync(id);
                if (busSchedule == null)
                {
                    return NotFound();
                }

                _context.BusSchedules.Remove(busSchedule);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        private bool BusScheduleExists(int id)
        {
            return _context.BusSchedules.Any(e => e.BusScId == id);
        }
    }
}
