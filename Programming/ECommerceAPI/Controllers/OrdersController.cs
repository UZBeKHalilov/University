using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase

    {

        private readonly ECommerceDbContext _context;


        public OrdersController(ECommerceDbContext context)

        {

            _context = context;

        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()

        {

            return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)

        {

            var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)

            {

                return NotFound();

            }

            return order;

        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)

        {

            // Validate customer existence
            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null)

            {

                return BadRequest("Customer not found.");

            }


            // Validate product stock and calculate total 
            decimal totalAmount = 0;
            foreach (var item in order.OrderItems)

            {

                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)

                {

                    return BadRequest($"Insufficient stock for product ID {item.ProductId}");

                }

                item.Price = product.Price * item.Quantity;
                totalAmount += item.Price;

                // Deduct stock
                product.Stock -= item.Quantity;

            }

            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);

        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)

        {

            if (id != order.Id)

            {

                return BadRequest();

            }

            _context.Entry(order).State = EntityState.Modified;

            try

            {

                await _context.SaveChangesAsync();

            }

            catch (DbUpdateConcurrencyException)

            {


                if (!OrderExists(id))

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

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)

        {

            var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)

            {

                return NotFound();

            }

            // Return stock for deleted order items 
            foreach (var item in order.OrderItems)

            {

                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)

                {

                    product.Stock += item.Quantity;


                }

            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool OrderExists(int id)

        {

            return _context.Orders.Any(o => o.Id == id);


        }

    }

}