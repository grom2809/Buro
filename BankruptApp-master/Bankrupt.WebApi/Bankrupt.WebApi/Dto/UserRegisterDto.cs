using System.ComponentModel.DataAnnotations;

namespace Bankrupt.WebApi.Dto
{
    public class UserRegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateOnly Birthdate { get; set; }
    }
}
