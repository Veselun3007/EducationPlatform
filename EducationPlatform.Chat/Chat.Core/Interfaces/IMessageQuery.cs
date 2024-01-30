using EPChat.Domain.Entities;

namespace EPChat.Core.Interfaces
{
    public interface IMessageQuery
    {
        IEnumerable<Message> GetFirstPackMessage(int chatId);

        IEnumerable<Message> GetNextPackMessage(int chatId, int oldestMessageId);
    }
}
