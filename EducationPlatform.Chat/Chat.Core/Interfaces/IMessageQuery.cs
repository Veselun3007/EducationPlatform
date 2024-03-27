namespace EPChat.Core.Interfaces
{
    public interface IMessageQuery<T>
    {
        Task<IEnumerable<T>> GetFirstPackMessageAsync(int chatId);

        Task<IEnumerable<T>> GetNextPackMessageAsync(int chatId, int oldestMessageId);
    }
}
