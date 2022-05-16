using System.ComponentModel.DataAnnotations;

namespace LoginDemo.Shared.Models
{
    public class AddUser
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string? Password { get; set; }

        [Required]
        public short Role {get; set;}
    }
}