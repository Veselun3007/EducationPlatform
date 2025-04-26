using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Services.ContentServices;
using CourseContent.Core.Services.FileServices;
using CourseContent.Core.Services.LinkServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CourseContent.Core
{
    public static class CoreDIExtention
    {
        public static void AddCoreServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IContentServices<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO>, AssignmentService>();
            builder.Services.AddScoped<IContentServices<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO>, MaterialService>();
            builder.Services.AddScoped<IContentServices<TopicDTO, TopicOutDTO, TopicUpdateDTO>, TopicService>();

            builder.Services.AddScoped<IFileServices<AssignmentfileOutDTO>, AssignmentFileService>();
            builder.Services.AddScoped<IFileServices<MaterialfileOutDTO>, MaterialFileService>();

            builder.Services.AddScoped<ILinkServices<AssignmentlinkOutDTO>, AssignmentLinkService>();
            builder.Services.AddScoped<ILinkServices<MateriallinkOutDTO>, MaterialLinkService>();
        }
    }
}
