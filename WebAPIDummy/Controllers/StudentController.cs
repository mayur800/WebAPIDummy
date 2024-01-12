using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebAPIDummy.Data;
using WebAPIDummy.Models;

namespace WebAPIDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ApplicationDbContext context,ILogger<StudentController>logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("Students are getting all studentdetails");
            return await _context.Students.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentByID(int id)
        {
            _logger.LogInformation("Students information are getting by id");
            var data = await _context.Students.FindAsync(id);
            if (data == null)
            {
                return NoContent();
            }
            return data;
        }
        [HttpPost]
        public async Task<ActionResult<Student>>PostStudent(Student student)
        {
            _logger.LogInformation("Students are created Own Details");
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Student>>PutStudent(int id,Student student)
        {
            _logger.LogInformation("Student are Updated Informations");
            if(student == null)
            {
                return NotFound();
            }
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.StudentName = student.StudentName;
            existingStudent.Age = student.Age;
            existingStudent.Gender=student.Gender;
            existingStudent.StudentStatus = student.StudentStatus;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>>DeleteStudent(int id)
        {
            var data = await _context.Students.FindAsync(id);
            if(data == null)
            {
                return NotFound();
            }
            _context.Students.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
