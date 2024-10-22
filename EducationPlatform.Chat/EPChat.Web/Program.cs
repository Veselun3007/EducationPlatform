using EPChat.Core.DTO.Request;
using EPChat.Core.DTO.Response;
using EPChat.Core.Helpers;
using EPChat.Core.Interfaces;
using EPChat.Core.Models.Config;
using EPChat.Core.Models.ErrorModels;
using EPChat.Core.Models.HelperModel;
using EPChat.Core.Services;
using EPChat.Infrastructure;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using EPChat.Web.Hubs;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            builder.Services.AddAWS(_configuration);
            builder.Services.Configure<AwsOptions>(_configuration.GetSection(nameof(AwsOptions)))
                            .Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));

            var (awsOptions, dbOptions) = ServiceExtensions.AddVariables(_configuration);

            builder.Services.AddDbContextPool<EducationPlatformContext>(options =>
            {
                options.UseNpgsql(dbOptions.ConnectionString);
            });
            builder.Services.AddScoped<AwsHelper>();
            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOperation<MessageDTO, MessageUpdateDTO, MessageOutDTO, MessageMediaOutDTO, MediaMessage, Error>, OperationServices>();
            builder.Services.AddScoped<IQuery<MessageOutDTO>, QueryService>();
            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));          
            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 102400000; // 100MB
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.MapHub<ChatHub>("/chat");

            app.Run();
        }
    }
}
