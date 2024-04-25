using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using FileAWS;
using Microsoft.Extensions.DependencyInjection;

namespace CourseService.Infrastructure.AWS {
    public static class AWSServiceExtensions {
        public static IServiceCollection AddS3(this IServiceCollection services) {
            //var awsOptions = new AWSOptions() {
            //    Credentials = new EnvironmentVariablesAWSCredentials(),
            //    Region = new EnvironmentVariableAWSRegion().Region
            //};
            //services.AddDefaultAWSOptions(awsOptions);
            //services.AddAWSService<AmazonS3Client>();
            //services.AddScoped<AmazonS3>();


            services.AddScoped<AmazonS3>();
            return services;
        }
    }
}
