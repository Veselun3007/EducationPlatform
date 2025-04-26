using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IAwsFileService
    {
        Task<string> AddFileAsync(IFormFile file);
        Task DeleteFileAsync(string name);
        Task<string> GetFileLink(string fileName);
    }
}