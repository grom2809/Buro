namespace Bankrupt.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateOnly Birthdate { get; set; }

        public Role Role { get; set; }
    }
}
