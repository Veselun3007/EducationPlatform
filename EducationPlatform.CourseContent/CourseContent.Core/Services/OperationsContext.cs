using CourseContent.Core.Interfaces;

namespace CourseContent.Core.Services
{
    public class OperationsContext<T>(IOperation<T> crudStrategy)
    {

        private readonly IOperation<T> _crudStrategy = crudStrategy;

        protected internal async Task<T> Create(T entity)
        {
            return await _crudStrategy.CreateAsync(entity);
        }

        protected internal async Task<bool> Delete(int id)
        {
            return await _crudStrategy.DeleteAsync(id);
        }

        protected internal async Task<IEnumerable<T>> GetAll()
        {
            return await _crudStrategy.GetAllAsync();
        }

        protected internal async Task<T> GetById(int id)
        {
            return await _crudStrategy.GetByIdAsync(id);
        }

        protected internal async Task<T> Update(int id, T entity)
        {
            return await _crudStrategy.UpdateAsync(id, entity);
        }

        protected internal async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            await _crudStrategy.RemoveRangeAsync(entities);
        }
    }
}
