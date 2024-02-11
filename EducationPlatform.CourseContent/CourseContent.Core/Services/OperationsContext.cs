using CourseContent.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services
{
    public class OperationsContext<T>(IOperation<T> crudStrategy)
    {

        private readonly IOperation<T> _crudStrategy = crudStrategy;

        public async Task<T> CreateAsync(T entity, List<IFormFile> files)
        {
            return await _crudStrategy.CreateAsync(entity, files);
        }

        public async Task DeleteAsync(int id)
        {
            await _crudStrategy.DeleteAsync(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _crudStrategy.GetByIdAsync(id);
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            return await _crudStrategy.UpdateAsync(id, entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            await _crudStrategy.RemoveRangeAsync(entities);
        }

        public async Task<string?> GetFileByIdAsync(int id)
        {
            return await _crudStrategy.GetFileByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllByCourseAsync(int id)
        {
            return await _crudStrategy.GetAllByCourseAsync(id);
        }
    }
}
