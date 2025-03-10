namespace Identity.Core.DTO.Requests
{
    public class TokenRequestModel
    {
        public required string RefreshToken { get; set; }

        public required string Email { get; set; }
    }
}
