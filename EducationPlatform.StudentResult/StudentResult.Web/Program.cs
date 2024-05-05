using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StudentResult.Infrastructure.Context;
using StudentResult.Infrastructure.Repositories;
using StudentResult.Infrastructure.AWS;
using StudentResult.Application;
using StudentResult.Domain.Entities;
using StudentResult.Web.Middlewares;

namespace StudentResult.Web {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplication();
            builder.Services.AddS3();
            string ep_connection = _configuration.GetConnectionString("EducationPlatformConnection") ?? "defaultConnectionString";
            builder.Services
                .AddDbContext<EducationPlatformContext>(opt => opt.UseNpgsql(ep_connection))
                .AddUnitOfWork<EducationPlatformContext>()
                .AddCustomRepository<CourseUser, CourseuserRepository>()
                .AddCustomRepository<StudentAssignment, StudentAssignmentRepository>();
            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            //builder.Services.AddAuthentication(options => {
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options => {
            //    options.Authority = $"https://cognito-idp.us-east-1.amazonaws.com/us-east-1_PlemC1CS5";
            //    options.TokenValidationParameters = new() {
            //        ValidateIssuer = true,
            //        ValidIssuer = $"https://cognito-idp.us-east-1.amazonaws.com/us-east-1_PlemC1CS5",
            //        ValidateLifetime = true,
            //        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
            //        ClockSkew = TimeSpan.Zero,
            //        ValidateAudience = false
            //    };
            //});

            var app = builder.Build();
            app.UseCors("AllowAll");
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
