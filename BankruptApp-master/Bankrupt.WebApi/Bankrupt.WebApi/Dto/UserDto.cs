using System.ComponentModel.DataAnnotations;

namespace Bankrupt.WebApi.Dto
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Birthdate { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
