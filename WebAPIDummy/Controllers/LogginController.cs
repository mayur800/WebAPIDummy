using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIDummy.Data;
using WebAPIDummy.Models;

namespace WebAPIDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<LogginController> _logger;

        public LogginController(ApplicationDbContext context, IConfiguration config,ILogger<LogginController>logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogInformation("Email and Password are Requirered.");
                return BadRequest("Email and password are required.");
            }

            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.Email == email);

            if (registration == null)
            {
                return NotFound($"Registration with email {email} not found.");
            }

            if (!ValidatePassword(registration, password))
            {
                _logger.LogInformation($"Invalid password for the{email} Not Found");
                return Unauthorized("Invalid password.");
            }

            var token = GenerateJwtToken(registration);

            return Ok(new { token });
        }

        private bool ValidatePassword(Registration registration, string password)
        {
            return registration.Password == password;
        }

        private string GenerateJwtToken(Registration registration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, registration.Id.ToString()),
                new Claim(ClaimTypes.Email, registration.Email),
           
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

