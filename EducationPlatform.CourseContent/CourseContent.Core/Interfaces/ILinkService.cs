namespace CourseContent.Core.Interfaces
{
    public interface ILinkService<LOutDTO>
    {
        Task<LOutDTO> AddLinkAsync(string link, int id);
        Task DeleteLinkAsync(int linkId);
    }
}
