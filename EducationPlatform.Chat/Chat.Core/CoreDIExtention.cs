using Chat.Core.DTO.Request;
using Chat.Core.DTO.Response;
using Chat.Core.Interfaces;
using Chat.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chat.Core
{
    public static class CoreDIExtention
    {
        public static void AddCoreServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMessageService<MessageDTO, MessageUpdateDTO, MessageOutDTO>, MessageService>();
            builder.Services.AddScoped<IMediaSevice<MessageMediaOutDTO, MessageMediaDTO>, MediaService>();
        }
    }
}
