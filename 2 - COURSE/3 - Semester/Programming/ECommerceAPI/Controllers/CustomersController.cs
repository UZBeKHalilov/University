using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ECommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDTO>>(customers));
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return _mapper.Map<CustomerDTO>(customer);
        }

        // POST: api/Customers
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerCreateDTO customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Claim userId from JWT Token
            var userIdClaim = User.FindFirst(ClaimTypes.Name);

            if (userIdClaim == null)
                return Unauthorized("User ID claim is missing in the token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID in token.");

            // Create a Customer entity from the DTO and set the UserId
            var customer = _mapper.Map<Customer>(customerDto);
            customer.UserId = userId;

            // Save to the database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Return the created result
            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = customer.UserId },
                _mapper.Map<CustomerDTO>(customer)
            );
        }


        // PUT: api/Customers/5


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerCreateDTO customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _mapper.Map(customerDto, customer); // Map from DTO to existing customer entity

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
            return _context.Customers.Any(e => e.UserId == id);
        }
    }
}
