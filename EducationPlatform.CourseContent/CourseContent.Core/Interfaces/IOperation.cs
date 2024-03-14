using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IOperation<T, Error, E, F> : IBaseOperation<T, Error, E>
    {
        Task<Result<string, Error>> RemoveRangeAsync(List<int> entities);

        Task<Result<string, Error>> GetFileByIdAsync(int id);

        Task<Result<F, Error>> AddFileAsync(IFormFile formFile, int id);

        Task<Result<string, Error>> DeleteFileAsync(int id);
    }
}
