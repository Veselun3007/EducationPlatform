using Microsoft.AspNetCore.Http;

namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task<string> GetName(IFormFile file);

        Task<T?> UpdateAsync(T entity, int id);

        Task<bool> DeleteAsync(int id);

    }
}
