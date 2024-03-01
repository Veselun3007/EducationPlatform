namespace Identity.Domain.Config
{
    public class AwsOptions
    {
        public string Region { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string UserPoolId { get; set; } = String.Empty;
        public string BucketName { get; set; } = String.Empty;
    }
}
