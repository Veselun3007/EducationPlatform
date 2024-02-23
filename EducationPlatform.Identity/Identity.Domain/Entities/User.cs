namespace Identity.Domain.Entities
{
    public class User
    {
        public required string Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string? UserImage { get; set; }
    }
}
