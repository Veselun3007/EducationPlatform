using CourseContent.Core.Interfaces;

namespace CourseContent.Core.Services
{
    public class OperationsContext<T>(IOperation<T> crudStrategy)
    {

        private readonly IOperation<T> _crudStrategy = crudStrategy;

        public async Task<T> Create(T entity)
        {
            return await _crudStrategy.CreateAsync(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _crudStrategy.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _crudStrategy.GetAllAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _crudStrategy.GetByIdAsync(id);
        }

        public async Task<T> Update(int id, T entity)
        {
            return await _crudStrategy.UpdateAsync(id, entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            await _crudStrategy.RemoveRangeAsync(entities);
        }

        public async Task<string?> GetFileById(int id)
        {
            return await _crudStrategy.GetFileByIdAsync(id);
        }
    }
}
