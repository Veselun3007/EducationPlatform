using IdentityServer.Core.Interfaces;
using IdentityServer.Core.Services;
using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure;
using IdentityServer.Infrastructure.Context;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Web.Helpers;
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
            var _secret = _configuration["JWT:Key"];
            var _issuer = _configuration["JWT:Issuer"];
            var _audience = _configuration["JWT:Audience"];
            var _identityConnectionString = _configuration["ConnectionStrings:IdentityDb"];
            var _businessConnectionString = _configuration["ConnectionStrings:EducationPlatformDB"];


            builder.Services.AddDbContext<IdentityDBContext>(options =>
            {
                options.UseNpgsql(_identityConnectionString);
            });

            builder.Services.AddDbContext<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(_businessConnectionString);
            });

            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<TokenHelper>();
            builder.Services.AddScoped<CryptographyHelper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBusinessUserOperation, UserOperation>();

            builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                //config.Password.RequiredLength = 8;
                //config.Password.RequireDigit = true;
                //config.Password.RequireLowercase = true;
                //config.Password.RequireUppercase = true;
                //config.Password.RequireNonAlphanumeric = true;
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
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
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
            //TODO: Remove it for production
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultControllerRoute();
            app.UseIdentityServer();

            app.Run();
        }
    }
}

