using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
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

        public async Task<bool> DeleteAsync(int id)
        {
            return await _crudStrategy.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _crudStrategy.GetAllAsync();
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

        public async Task<string?> GetFileById(int id)
        {         
            return  await _crudStrategy.GetFileByIdAsync(id);
        }

        public IQueryable<T> GetByCourse(int id)
        {
            return _crudStrategy.GetByCourse(id);
        }
    }
}
