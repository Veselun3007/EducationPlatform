using EPChat.Domain.Enums;
using EPChat.Domain.Interfaces;

namespace EPChat.Core.Interfaces
{
    public interface IOperation<T, E> where T : class, IEntity
    {
        Task<T> AddAsync(T message);

        Task<T?> EditAsync(int id, T message);
    
        Task<bool> DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions);

        Task<bool> RemoveRangeAsync(List<int> entitiesToDelete, DeleteOptionsEnum deleteOptions);

        Task<E?> GetMediaByIdAsync(int id);
    }
}
