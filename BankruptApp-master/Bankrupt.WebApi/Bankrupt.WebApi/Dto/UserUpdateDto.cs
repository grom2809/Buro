using System.ComponentModel.DataAnnotations;

namespace Bankrupt.WebApi.Dto
{
    public class UserUpdateDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateOnly Birthdate { get; set; }

        public string Password { get; set; }
    }
}
