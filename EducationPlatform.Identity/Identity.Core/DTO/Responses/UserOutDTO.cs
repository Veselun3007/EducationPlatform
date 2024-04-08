namespace Identity.Core.DTO.Responses
{
    public class UserOutDTO
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string? UserImage { get; set; }
    }
}
