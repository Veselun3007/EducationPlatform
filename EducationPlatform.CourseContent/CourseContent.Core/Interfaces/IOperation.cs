using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IOperation<O, Error, I, F, U> : IBaseOperation<O, Error, I, U>
    {
        Task<Result<F, Error>> AddFileAsync(IFormFile formFile, int id);

        Task<Result<string, Error>> AddLinkAsync(string link, int id);

        Task<Result<string, Error>> RemoveRangeAsync(List<int> entities);

        Task<Result<string, Error>> GetFileByIdAsync(int fileId);
        
        Task<Result<string, Error>> DeleteFileAsync(int fileId);

        Task<Result<string, Error>> DeleteLinkAsync(int linkId);
    }
}
