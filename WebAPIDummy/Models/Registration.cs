using System.ComponentModel.DataAnnotations;

namespace WebAPIDummy.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string? FirtName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
