using CSharpFunctionalExtensions;
using EPChat.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EPChat.Core.Interfaces
{
    public interface IOperation<I, U, O, M, F, Error> where I : class
    {
        Task<Result<O, Error>> AddAsync(I message);

        Task<Result<O?, Error>> EditAsync(U message);

        Task<Result<string?, Error>> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions);

        Task<Result<string?, Error>> GetMediaByIdAsync(int id);

        Task<Result<string?, Error>> DeleteFileAsync(int messageMediaId);

        Task<Result<M?, Error>> AddFileAsync(F file, int messageId);
    }
}
