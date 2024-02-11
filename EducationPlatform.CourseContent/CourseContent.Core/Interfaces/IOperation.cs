using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Interfaces
{
    public interface IOperation<T>
    {
        Task<T> CreateAsync(T entity, List<IFormFile> files);

        Task<T> UpdateAsync(int id, T entity);      

        Task<T?> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task<string?> GetFileByIdAsync(int id);

        Task<IEnumerable<T>> GetAllByCourseAsync(int courseId);
    }
}
