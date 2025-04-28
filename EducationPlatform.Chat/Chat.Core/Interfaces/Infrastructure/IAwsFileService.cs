using Microsoft.AspNetCore.Http;

namespace Chat.Core.Interfaces.Infrastructure
{
    public interface IAwsFileService
    {
        Task<string> AddFileAsync(IFormFile file);
        Task DeleteFileAsync(string name);
        Task<string> GetFileLink(string fileName);
    }
}