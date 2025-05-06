using Microsoft.AspNetCore.Http;

namespace Identity.Core.Interfaces
{
    public interface IAwsFileService
    {
        Task<string?> AddFileAsync(IFormFile file);
        Task DeleteFileAsync(string name);
        Task<string> GetFileLink(string fileName);
    }
}