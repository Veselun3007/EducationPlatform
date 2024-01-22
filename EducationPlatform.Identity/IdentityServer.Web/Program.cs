using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityServer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var secret = _configuration["Secret:SigningKey"];
            var isconnectionString = 
                _configuration["EducationPlatformConnectionStrings:IdentityDbConnectionStrings"];
            var epconnectionString = 
                _configuration["EducationPlatformConnectionStrings:EducationPlatformContext"];

            
            builder.Services.AddDbContext<IdentityDBContext>(options =>
            {
                options.UseNpgsql(isconnectionString);
            });

            builder.Services.AddDbContext<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(epconnectionString);
            });

            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireNonAlphanumeric = true;
                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<IdentityDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidIssuer = "BhavikCorp",
                    ValidAudience = "TestAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients) 
                .AddDeveloperSigningCredential();

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<IdentityDBContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception exception)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while app initialization");
                }
            } 

            app.MapDefaultControllerRoute();
            app.UseIdentityServer();

            app.Run();
        }
    }
}

