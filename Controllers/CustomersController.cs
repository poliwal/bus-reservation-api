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
    public class CustomersController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public CustomersController(BusReservationContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPut]
        [Route("deductFare")]
        public IActionResult DeductFare([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "fare")] decimal Fare)
        {
            var cust = _context.Customers.Where(c=>c.Cid == Cid).FirstOrDefault();

            cust.Wallet = cust.Wallet - Fare;

            _context.SaveChanges();

            return Ok("Deducted");
        }

        [HttpPut]
        [Route("change-password")]
        public IActionResult ChangePassword([FromQuery(Name = "cid")] long Cid, [FromQuery(Name = "cp")] string cp, [FromQuery(Name = "np")] string np, [FromQuery(Name = "cnp")] string cnp)
        {
            var cust = _context.Customers.Where(a => a.Cid == Cid).FirstOrDefault();
            if (cust.Password != cp) return BadRequest("wrong Current Password.");

            if (np != cnp)
            {
                return BadRequest("New passwords didn't matched.");
            }

            // update details if same
            cust.Password = cnp;

            _context.SaveChanges();
            return Ok("Password changed successfully.");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterCustomer([FromQuery(Name = "cp")] string Cp, Customer customer)
        {
            /*var cust = _context.Customers.Where(a => a.Cid == Cid).FirstOrDefault();*/
            /*if (customer.Password != Cp) return BadRequest("wrong Current Password.");*/

            if (customer.Password != Cp)
            {
                return Ok("Both passwords didn't matched.");
            }

            // update details if same
            _context.Customers.Add(customer);

            _context.SaveChanges();
            return Ok("Added Customer.");
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Cid)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Updated Profile");
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostCustomer(Customer customer)
        {
            var res = _context.Customers.Where(c => c.Email == customer.Email && c.ContactNo == customer.ContactNo).FirstOrDefault();

            if(res != null)
            {
                return Ok(res);
            }
            else
            {
                _context.Customers.Add(customer);
                _context.SaveChangesAsync();
            }
            

            return CreatedAtAction("GetCustomer", new { id = customer.Cid }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Cid == id);
        }



        [HttpPut]
        [Route("refundFare")]
        public IActionResult RefundFare([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "fare")] decimal Fare)
        {
            var cus = _context.Customers.Where(c => c.Cid == Cid).FirstOrDefault();

            cus.Wallet = cus.Wallet + Fare;
            _context.SaveChanges();
            return Ok("Refunded");
        }

        [Route("login")]
        [HttpGet]
        public IActionResult checkLogin(string email, string password)
        {
            try
            {
                var result = _context.Customers.Where(x => x.Email == email && x.Password==password).FirstOrDefault();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok("Invalid");
                }
            }
            catch (Exception e)
            {
                /*Console.Write(e);*/
                return Ok(e.Message);
            }
        }
    }
}
