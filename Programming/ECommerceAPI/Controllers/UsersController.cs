using ECommerceAPI.Data;
using ECommerceAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;
using AutoMapper;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly AuthSettings _authSettings;
        private readonly IMapper _mapper;

        public UserController(ECommerceDbContext context, IOptions<AuthSettings> authSettings, IMapper mapper)
        {
            _context = context;
            _authSettings = authSettings.Value;
            _mapper = mapper;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDTO userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
            {
                return BadRequest("Username already exists.");
            }

            userDto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordHash);
            userDto.Role = string.IsNullOrEmpty(userDto.Role) ? "Customer" : userDto.Role;

            var user = _mapper.Map<User>(userDto);           

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCreateDTO userDto)
        {
            var dbUser = await _context.Users.SingleOrDefaultAsync(u => u.Username ==
            userDto.Username);

            if (dbUser == null || !BCrypt.Net.BCrypt.Verify(userDto.PasswordHash,
            dbUser.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(dbUser);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_authSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),

                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}