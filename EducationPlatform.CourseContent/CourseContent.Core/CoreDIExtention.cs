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
            builder.Services.AddScoped<IContentService<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO>, AssignmentService>();
            builder.Services.AddScoped<IContentService<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO>, MaterialService>();
            builder.Services.AddScoped<IContentService<TopicDTO, TopicOutDTO, TopicUpdateDTO>, TopicService>();

            builder.Services.AddScoped<IFileService<AssignmentfileOutDTO>, AssignmentFileService>();
            builder.Services.AddScoped<IFileService<MaterialfileOutDTO>, MaterialFileService>();

            builder.Services.AddScoped<ILinkService<AssignmentlinkOutDTO>, AssignmentLinkService>();
            builder.Services.AddScoped<ILinkService<MateriallinkOutDTO>, MaterialLinkService>();
        }
    }
}
