namespace Identity.Core.DTO.Requests
{
    public class ConfirmEmailRequest
    {
        public required string Email { get; set; }

        public required string ConfirmCode { get; set; }
    }
}
