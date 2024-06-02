namespace Identity.Core.DTO.Requests
{
    public class ResetPasswordRequest
    {
        public required string Email { get; set; }

        public required string ConfirmCode { get; set; }

        public required string Password { get; set; }
    }
}
