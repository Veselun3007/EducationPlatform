using CourseContent.Core;
using CourseContent.Infrastructure;
using CourseContent.Web.Middlewares;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CourseContent.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.AddCoreServices();
            var awsOptions = builder.AddInfrastructure(_configuration);
            ServiceExtensions.AddJwtValidation(builder, awsOptions);

            builder.Services.AddControllers();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CourseContent", Version = "v2" });
                options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Authentication Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JsonWebToken",
                    Scheme = "Bearer"
                });
            });
            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseCors("AllowAll");
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
