namespace Identity.Core.DTO.User
{
    public class ConfirmEmailRequest
    {
        public required string Email { get; set; }

        public required string ConfirmCode { get; set; }
    }
}
