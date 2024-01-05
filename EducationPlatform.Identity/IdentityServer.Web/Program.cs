using IdentityServer.Domain.Entities;
using IdentityServer.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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


            builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
                config.Password.RequireNonAlphanumeric = false;
                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<IdentityDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients) 
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder
                    .UseNpgsql(isconnectionString, sql => sql
                    .MigrationsAssembly(typeof(Program).Assembly.FullName));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                });

           
            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "EducationPlatformAuth.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });
            
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

