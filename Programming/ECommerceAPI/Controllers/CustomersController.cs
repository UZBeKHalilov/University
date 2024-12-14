using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase

    {

        private readonly ECommerceDbContext _context;

        public CustomersController(ECommerceDbContext context)

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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)

        {

            if (!IsValidEmail(customer.Email))

            {

                return BadRequest("Invalid email format.");

            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);

        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)

        {

            if (id != customer.Id)

            {

                return BadRequest();

            }

            if (!IsValidEmail(customer.Email))

            {


                return BadRequest("Invalid email format.");

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

            return NoContent();

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

            return _context.Customers.Any(e => e.Id == id);

        }


        private bool IsValidEmail(string email)

        {

            return email.Contains("@") && email.Contains(".");

        }

    }
}
