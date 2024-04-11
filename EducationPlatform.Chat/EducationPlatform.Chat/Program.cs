using EPChat.Core.Interfaces;
using EPChat.Core.Services;
using EPChat.Domain.Entities;
using EPChat.Infrastructure;
using EPChat.Infrastructure.Contexts;
using EPChat.Infrastructure.Interfaces;
using EPChat.Web.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace EducationPlatform.Chat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var _configuration = builder.Configuration;

            string chatConnection = _configuration.GetConnectionString("ChatConnection") ?? "defaultConnectionString";
            
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            builder.Services.AddDbContextPool<ChatDBContext>(options =>
            {
                options.UseMongoDB(chatConnection, "ChatDB");
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOperation<Message, MessageMedia>, OperationServices>();
            builder.Services.AddScoped<IQuery<Message, ChatMember>, QueryService>();

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

            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwagger();
            }

            app.MapHub<ChatHub>("/Chat");

            app.Run();
        }
    }
}
