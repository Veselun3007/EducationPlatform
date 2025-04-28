using Chat.Core;
using Chat.Infrastructure;
using Chat.Web.Filters;
using Chat.Web.Hubs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;

namespace Chat.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.AddCoreServices();
            var awsOptions = builder.AddInfrastructure(_configuration);
            builder.Services.AddSingleton<GlobalHubExceptionFilter>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat", Version = "v2" });
                options.AddSignalRSwaggerGen();
            });
            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            builder.Services.AddSignalR(options =>
            {
                options.AddFilter<GlobalHubExceptionFilter>();
                options.MaximumReceiveMessageSize = 102400000; // 100MB
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });

            var app = builder.Build();
            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
