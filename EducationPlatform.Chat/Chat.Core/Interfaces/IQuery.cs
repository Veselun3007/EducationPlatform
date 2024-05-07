namespace EPChat.Core.Interfaces
{
    public interface IQuery<O>
    {
        Task<IEnumerable<O>> GetFirstPackMessageAsync(int chatId);

        Task<IEnumerable<O>> GetNextPackMessageAsync(int chatId, int oldestMessageId);
    }
}
