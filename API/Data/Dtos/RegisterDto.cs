
using System.ComponentModel.DataAnnotations;

namespace API.Data.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}