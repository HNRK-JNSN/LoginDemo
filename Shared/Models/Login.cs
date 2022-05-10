using System.ComponentModel.DataAnnotations;

namespace LoginDemo.Shared.Models
{
    public class Login
    {
        [Required]
        public string? EmailAddress { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}