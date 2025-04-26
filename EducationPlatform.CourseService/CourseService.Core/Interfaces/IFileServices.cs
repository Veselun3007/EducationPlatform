using Microsoft.AspNetCore.Http;

namespace CourseService.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> AddFileAsync(IFormFile file);
        Task DeleteFileAsync(string name);
        Task<string> GetFileLink(string fileName);
    }
}