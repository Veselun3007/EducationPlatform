using EPChat.Domain.Entities;
using EPChat.Domain.Enums;

namespace EPChat.Core.Interfaces
{
    public interface IMessageOperation
    {
        void Add(Message message);

        void Edit(Message message);
    
        Task<bool> DeleteMessage(int messageId, DeleteOptionsEnum deleteOptions);

        bool DeleteRange(IEnumerable<Message> entitiesToDelete, DeleteOptionsEnum deleteOptions);

        Task<MessageMedia?> GetByIdAsync(int id);
    }
}
