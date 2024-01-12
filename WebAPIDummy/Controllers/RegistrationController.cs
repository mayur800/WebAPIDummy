using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDummy.Data;
using WebAPIDummy.Models;

namespace WebAPIDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegistrationController> _logger;
        public RegistrationController(ApplicationDbContext context,ILogger<RegistrationController>logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistration()
        {
            _logger.LogInformation("Getting All details Register hear");
            return await _context.Registrations.ToListAsync();
            
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            _logger.LogInformation("Getting All details Register By Id");
            var data = await _context.Registrations.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult<Registration>> PostRegistration(Registration registration)
        {
            _logger.LogInformation("Create the Register User");
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return Ok(registration);

        }
        [HttpPut]
        public async Task<ActionResult<Registration>> PutRegistration(int id, Registration registration)
        {
            _logger.LogInformation("Update the user in Registration");
            if (registration == null)
            {
                return NotFound();
            }
            var existingRegistration = await _context.Registrations.FindAsync(id);
            if (existingRegistration == null)
            {
                return NotFound();
            }
            existingRegistration.Id = registration.Id;
            existingRegistration.FirtName = registration.FirtName;
            existingRegistration.LastName = registration.LastName;
            existingRegistration.Email = registration.Email;
            existingRegistration.Password = registration.Password;
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult<Registration>> DeleteRegistration(int id)
        {
            _logger.LogInformation("Delete the User From the Registration");
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {

                return NotFound();
            }
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
