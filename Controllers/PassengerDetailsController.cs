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

        #region Get all Passengers
        // GET: api/PassengerDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerDetail>>> GetPassengerDetails()
        {
            try
            {
                return await _context.PassengerDetails.ToListAsync();
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get Passenger by PassengerId
        // GET: api/PassengerDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDetail>> GetPassengerDetail(int id)
        {
            try
            {
                var passengerDetail = await _context.PassengerDetails.FindAsync(id);

                if (passengerDetail == null)
                {
                    return NotFound();
                }

                return passengerDetail;
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Update a passenger's details
        // PUT: api/PassengerDetails/5
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
        #endregion


        #region Add Passengers list
        // POST: api/PassengerDetails
        [HttpPost]
        public async Task<ActionResult<PassengerDetail>> PostPassengerDetail(List<PassengerDetail> passengerDetail)
        {
            try
            {
                foreach (var item in passengerDetail)
                {
                    _context.PassengerDetails.Add(item);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPassengerDetail", new { id = passengerDetail }, passengerDetail);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete a Passenger
        // DELETE: api/PassengerDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassengerDetail(int id)
        {
            try
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
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion

        private bool PassengerDetailExists(int id)
        {
            return _context.PassengerDetails.Any(e => e.PassId == id);
        }



        #region Get passenger by bookingId
        [HttpGet]
        [Route("getPassenger")]
        public IActionResult GetPassenger([FromQuery(Name = "bookingid")] int bookingid)
        {
            try
            {
                dynamic passenger = (from p in _context.PassengerDetails
                                     where p.BookingId == bookingid
                                     select p).ToList();

                return Ok(passenger);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get passenger's seatno by bookingId
        [HttpGet]
        [Route("getPassengerSeatNo")]
        public IActionResult GetPassengerSeatNo([FromQuery(Name = "bookingid")] int bookingid)
        {
            try
            {
                dynamic seatno = (from p in _context.PassengerDetails
                                  where p.BookingId == bookingid
                                  select p.SeatNo).ToList();

                return Ok(seatno);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion



        #region Get passenger's return seat no
        [HttpGet]
        [Route("getPassengerReturnSeatNo")]
        public IActionResult GetPassengerReturnSeatNo([FromQuery(Name = "bookingid")] int bookingid)
        {
            try
            {
                dynamic returnSeatNo = (from p in _context.PassengerDetails
                                        where p.BookingId == bookingid
                                        select p.ReturnSeatNo).ToList();
                return Ok(returnSeatNo);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            
        }
        #endregion
    }
}
