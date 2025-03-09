namespace CourseContent.Core.Interfaces
{
    public interface ILinkServices<LOutDTO>
    {
        Task<LOutDTO> AddLinkAsync(string link, int id);
        Task DeleteLinkAsync(int linkId);
    }
}
