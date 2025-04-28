using Chat.Domain.Enums;

namespace Chat.Core.Interfaces
{
    public interface IMessageService<IMessage, UMessage, OMessage> where IMessage : class
    {
        Task<OMessage> AddAsync(IMessage message);

        Task<OMessage?> EditAsync(UMessage message);

        Task DeleteAsync(int messageId, DeleteOptionsEnum deleteOptions);

        Task<IEnumerable<OMessage>> GetFirstPackMessageAsync(int courseId);

        Task<IEnumerable<OMessage>> GetNextPackMessageAsync(int courseId, int oldestMessageId);
    }
}
