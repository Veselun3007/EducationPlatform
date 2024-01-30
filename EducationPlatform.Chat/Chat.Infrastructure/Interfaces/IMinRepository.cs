using EPChat.Domain.Interfaces;

namespace EPChat.Infrastructure.Interfaces
{
    public interface IMinRepository<T> where T : class, IEntity
    {
        Task<T?> GetById(int id);
    }
}
