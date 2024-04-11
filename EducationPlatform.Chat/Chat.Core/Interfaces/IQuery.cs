using System.Linq.Expressions;

namespace EPChat.Core.Interfaces
{
    public interface IQuery<T, E>
    {
        Task<IEnumerable<T>> GetFirstPackMessageAsync(int chatId);

        Task<IEnumerable<T>> GetNextPackMessageAsync(int chatId, int oldestMessageId);

        Task<IEnumerable<E>> GetMembersAsync(Expression<Func<E, bool>> filter);
    }
}
