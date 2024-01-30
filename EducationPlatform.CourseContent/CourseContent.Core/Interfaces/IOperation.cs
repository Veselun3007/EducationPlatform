using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IOperation<T>
    {
        Task<T> CreateAsync(T entity, List<IFormFile> files);

        Task<T> UpdateAsync(int id, T entity);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> GetByCourse(int id);

        Task<T> GetByIdAsync(int id);

        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task<string?> GetFileByIdAsync(int id);
    }
}
