namespace EPChat.Core.Interfaces
{
    public interface IQuery<O>
    {
        Task<IEnumerable<O>> GetFirstPackMessageAsync(int courseId);

        Task<IEnumerable<O>> GetNextPackMessageAsync(int courseId, int oldestMessageId);
    }
}
