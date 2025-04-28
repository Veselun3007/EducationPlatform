namespace Chat.Core.Interfaces
{
    public interface IMediaSevice<OMedia, IMedia>
    {
        Task DeleteFileAsync(int messageMediaId);

        Task<OMedia?> AddFileAsync(IMedia file, int messageId);

        Task<string?> GetMediaByIdAsync(int id);
    }
}
