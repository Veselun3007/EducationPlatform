namespace CourseContent.Core.Interfaces
{
    public interface IOperation<T>
    {
        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(int id, T entity);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task RemoveRangeAsync(IEnumerable<T> entities);

        Task<string?> GetFileByIdAsync(int id);
    }
}
