using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IFileServices<FOutDTO>
    {
        Task<string?> GetFileByIdAsync(int fileId);

        Task<FOutDTO> AddFileAsync(IFormFile formFile, int id);

        Task DeleteFileAsync(int fileId);
    }
}
