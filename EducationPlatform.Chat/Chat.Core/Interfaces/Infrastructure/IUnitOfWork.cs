using Chat.Domain.Entities;

namespace Chat.Core.Interfaces.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Message, int> MessageRepository { get; }

        IMinRepository<MessageMedia, int> MessageMediaRepository { get; }

        Task<int> CommitAsync();
    }
}
