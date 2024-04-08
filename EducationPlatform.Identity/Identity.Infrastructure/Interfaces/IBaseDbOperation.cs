namespace Identity.Infrastructure.Interfaces
{
    public interface IBaseDbOperation<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task<T?> UpdateAsync(T entity, string id);

        Task DeleteAsync(string id);

        Task<T?> GetByIdAsync(string id);
    }
}
