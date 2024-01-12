using Microsoft.EntityFrameworkCore;
using WebAPIDummy.Models;

namespace WebAPIDummy.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>option):base(option)
        {
            
        }
        public virtual DbSet<Registration>Registrations { get; set; }
        public virtual DbSet<Student>Students { get; set; } 
    }
}
