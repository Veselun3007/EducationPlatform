namespace CourseService.Domain.Config
{
    public class AwsOptions
    {
        public string Region { get; set; } = string.Empty;
        public string UserPoolId { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
    }
}
