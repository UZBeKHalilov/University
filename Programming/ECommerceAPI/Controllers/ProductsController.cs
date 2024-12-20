using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtirAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;


        public ProductsController(ECommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            // Mahsulotlarni Category bilan birga yuklash
            var products = await _context.Products
                                         .Include(p => p.Category) // Categoryni qo'shish
                                         .ToListAsync();

            // DTOga mapping qilish
            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }
    

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            // Mahsulotni Category bilan birga yuklash
            var product = await _context.Products
                                        .Include(p => p.Category) // Categoryni qo'shish
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // DTOga mapping qilish
            return _mapper.Map<ProductDTO>(product);
        }


        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct([FromBody] ProductCreateDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest("The specified category does not exist.");
            }

            product.Category = category;

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while saving the product.");
            }

            var savedProduct = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            return CreatedAtAction(nameof(GetProduct), new { id = savedProduct.Id },
                                   _mapper.Map<ProductDTO>(savedProduct));
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
