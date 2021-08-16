using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusReservation.Models;
using System.Net.Mail;
using System.Net;

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

        #region Get all Customers
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        #endregion


        #region Get Customer by Cid
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return customer;
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Get CustomerId for a email
        // GET: api/Customers/5
        [HttpGet]
        [Route("getCidByEmail")]
        public IActionResult GetCustomerIdForEmail(string Email)
        {
            try
            {
                var customer = _context.Customers.Where(c => c.Email == Email).Select(c => c.Cid).FirstOrDefault();

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }

        }
        #endregion

        #region Deduct Fare on Booking
        [HttpPut]
        [Route("deductFare")]
        public IActionResult DeductFare([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "fare")] decimal Fare)
        {
            try
            {
                var cust = _context.Customers.Where(c => c.Cid == Cid).FirstOrDefault();

                cust.Wallet = cust.Wallet - Fare;

                _context.SaveChanges();

                return Ok("Deducted");
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Forgot Password SMTP
        [HttpGet]
        [Route("forgotPassword")]
        public IActionResult ForgotPassword([FromQuery(Name = "email")] string Email)
        {
            var cust = _context.Customers.Where(c => c.Email == Email).FirstOrDefault();
            if (cust == null)
            {
                return BadRequest("invalid email");
            }

            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("ltibusreservation@gmail.com", "Mihir@123"),
            };

            string subject = "Reset your Password";

            string body = "Reset Password Link :  http://localhost:4200/reset-password ";
            try
            {
                email.Send("ltibusreservation@gmail.com", Email, subject, body);
                return Ok("Email sent");
            }
            catch (Exception e)
            {
                return BadRequest("Email not sent");
            }
        }
        #endregion

        #region Get Customers with no Reservation
        [HttpGet]
        [Route("getCustomersWithNoReservation")]
        public IActionResult GetCustomersWithNoReservation()
        {
            try
            {
                var customers = (from c in _context.Customers
                                 where c.HasBooked == false
                                 select c).ToList();

                return Ok(customers);
            }
            catch (Exception e)
            {

                return NotFound(e.Message);
            }

        }
        #endregion

        #region Change Password
        [HttpPut]
        [Route("change-password")]
        public IActionResult ChangePassword([FromQuery(Name = "cid")] long Cid, [FromQuery(Name = "cp")] string cp, [FromQuery(Name = "np")] string np, [FromQuery(Name = "cnp")] string cnp)
        {
            try
            {
                var cust = _context.Customers.Where(a => a.Cid == Cid).FirstOrDefault();
                if (cust.Password != EncryptPassword(cp)) return BadRequest("wrong Current Password.");

                if (np != cnp)
                {
                    return BadRequest("New passwords didn't matched.");
                }

                // update details if same
                cust.Password = EncryptPassword(cnp);

                _context.SaveChanges();
                return Ok("Password changed successfully.");
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Reset Password
        [HttpPut]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "np")] string np, [FromQuery(Name = "cnp")] string cnp)
        {
            try
            {
                var cust = _context.Customers.Where(a => a.Cid == Cid).FirstOrDefault();

                if (np != cnp)
                {
                    return BadRequest("New passwords didn't matched.");
                }

                // update details if same
                cust.Password = EncryptPassword(cnp);

                _context.SaveChanges();
                return Ok("Password changed successfully.");
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }

        }
        #endregion


        #region Registering Customer
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterCustomer([FromQuery(Name = "cp")] string Cp, Customer customer)
        {
            try
            {

                var cust = _context.Customers.Where(a => a.Email == customer.Email && a.IsAuthorized == true).FirstOrDefault();
                
                /*if (customer.Password != Cp) return BadRequest("wrong Current Password.");*/
                if (cust != null)
                {
                    return Ok("Email Already Exists.");
                }
                else
                {
                    var unauthCust = _context.Customers.Where(a => a.Email == customer.Email && a.IsAuthorized == false).FirstOrDefault();
                    if(unauthCust != null)
                    {
                        if (customer.Password != Cp)
                        {
                            return Ok("Both passwords didn't matched.");
                        }
                        unauthCust.Fname = customer.Fname;
                        unauthCust.Lname = customer.Lname;
                        unauthCust.Password = EncryptPassword(customer.Password);
                        unauthCust.ContactNo = customer.ContactNo;
                        unauthCust.Wallet = customer.Wallet;
                        unauthCust.IsAuthorized = customer.IsAuthorized;
                        /*_context.Customers.Update(unauthCust);*/
                        _context.SaveChanges();
                        return Ok("Added Customer.");
                    }
                    else
                    {
                        if (customer.Password != Cp)
                        {
                            return Ok("Both passwords didn't matched.");
                        }

                        customer.Password = EncryptPassword(customer.Password);

                        // update details if same
                        _context.Customers.Add(customer);

                        _context.SaveChanges();
                        return Ok("Added Customer.");
                    }
                }

                
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Update Customer
        // PUT: api/Customers/5
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
        #endregion

        #region Add Unauthorized Customer
        // POST: api/Customers
        [HttpPost]
        public IActionResult PostCustomer(Customer customer)
        {
            try
            {
                var res = _context.Customers.Where(c => c.Email == customer.Email).FirstOrDefault();

                if (res != null)
                {
                    return Ok(res);
                }
                else
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }


                return CreatedAtAction("GetCustomer", new { id = customer.Cid }, customer);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Delete Customer
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
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
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Cid == id);
        }


        #region Refund Fare on Cancellation
        [HttpPut]
        [Route("refundFare")]
        public IActionResult RefundFare([FromQuery(Name = "cid")] int Cid, [FromQuery(Name = "fare")] decimal Fare)
        {
            try
            {
                var cus = _context.Customers.Where(c => c.Cid == Cid).FirstOrDefault();

                cus.Wallet = cus.Wallet + Fare;
                _context.SaveChanges();
                return Ok("Refunded");
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
            
        }
        #endregion

        #region Login Customer
        [Route("login")]
        [HttpGet]
        public IActionResult checkLogin(string email, string password)
        {
            try
            {
                var result = _context.Customers.Where(x => x.Email == email && x.Password== EncryptPassword(password)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Invalid");
                }
            }
            catch (Exception e)
            {
                /*Console.Write(e);*/
                return Ok(e.Message);
            }
        }
        #endregion

        #region Password Encryption
        private string EncryptPassword(string password)
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encryptedPassword = Convert.ToBase64String(encData_byte);
            return encryptedPassword;
        }
        #endregion

        #region Password Decryption
        private string DecryptPassword(string encryptedPassword)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decoder = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptedPassword);
            int charCount = utf8Decoder.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            string decryptedPassword = new string(decoded_char);
            return decryptedPassword;
        }
        #endregion
    }
}
