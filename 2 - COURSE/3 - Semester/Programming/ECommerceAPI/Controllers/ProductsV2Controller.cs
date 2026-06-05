using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ECommerceAPI.Data;
using AutoMapper;


[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsV2Controller : ControllerBase
{

    private readonly ECommerceDbContext _context;
    private readonly IMapper _mapper;

    public ProductsV2Controller(ECommerceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    /// <summary>
    /// Gets products for API version 2.0.
    /// </summary>
    /// <returns>A message indicating this is version 2.0 of the Products API.</returns>
    [HttpGet]
    public ActionResult<string> GetProducts()
    {
        return "This is version 2.0 of the Products API.";
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> getProductsByCategoryId(int categoryId)
    {
        // Mahsulotlarni Category bilan birga yuklash
        var products = await _context.Products
                                     .Include(p => p.Category)
                                     .Where(p => p.CategoryId == categoryId)
                                     .ToListAsync();

        // DTOga mapping qilish
        return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
    }
}
