using Microsoft.AspNetCore.Http;

namespace StudentResult.Application.Interfaces
{
    public interface IAwsFileService
    {
        Task<string?> AddFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string name);
        Task<string> GetFileLink(string fileName);
    }
}
