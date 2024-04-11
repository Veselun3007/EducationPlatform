using EPChat.Domain.Interfaces;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IRepository<T> : IGetRepository<T>, IMinRepository<T> where T : class, IEntity
    {
        Task<T?> UpdateAsync(int id, T entity);

        Task RemoveRangeAsync(List<int> entities);       
    }
}
