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

        #region Get all Seats of all Buses
        // GET: api/BusSeatNoes
        [HttpGet]
        public IActionResult GetBusSeatNos()
        {
            try
            {
                return Ok((from seat in _context.BusSeatNos
                           orderby seat.SeatNo
                           select seat).ToList());
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion


        #region Get All Seats for a Bus
        // GET: api/BusSeatNoes/5
        [HttpGet("{id}")]
        public IActionResult GetBusSeatNo(int id)
        {
            try
            {
                var busSeatNo = _context.BusSeatNos.Where(b => b.BusScId == id).OrderBy(b => b.SeatNo).ToList();

                if (busSeatNo == null)
                {
                    return NotFound();
                }

                return Ok(busSeatNo);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion


        #region Update a seat of a Bus
        // PUT: api/BusSeatNoes/5
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
        #endregion

        #region Add all seats for a Bus
        // POST: api/BusSeatNoes
        [HttpPost]
        public IActionResult PostBusSeatNo(List<BusSeatNo> busSeatNo)
        {
            try
            {
                foreach (var item in busSeatNo)
                {
                    _context.BusSeatNos.Add(item);
                }

                _context.SaveChangesAsync();

                return Ok("Added Seats)");
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete a seat
        // DELETE: api/BusSeatNoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusSeatNo(int id)
        {
            try
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
            catch (Exception e)
            {

                return Ok(e.Message);
            }   
        }
        #endregion

        private bool BusSeatNoExists(int id)
        {
            return _context.BusSeatNos.Any(e => e.SeatId == id);
        }
    }
}
