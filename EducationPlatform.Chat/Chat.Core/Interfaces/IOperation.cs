using CSharpFunctionalExtensions;
using EPChat.Domain.Enums;

namespace EPChat.Core.Interfaces
{
    public interface IOperation<I, U, O, M, Error> where I : class
    {
        Task<Result<O, Error>> AddAsync(I message);

        Task<Result<O?, Error>> EditAsync(U message);

        Task<Result<string?, Error>> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions);

        Task<Result<string?, Error>> RemoveRangeAsync(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions);

        Task<Result<string?, Error>> GetMediaByIdAsync(int id);
    }
}
