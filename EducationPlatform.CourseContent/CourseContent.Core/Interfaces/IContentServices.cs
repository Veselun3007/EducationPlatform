namespace CourseContent.Core.Interfaces
{  
    public interface IContentServices<TInDTO, TOutDTO, TUpdateDTO> 
    {
        Task<IEnumerable<TOutDTO>?> GetAllByCourseAsync(int courseId);

        Task<TOutDTO?> CreateAsync(TInDTO entity);

        Task<TOutDTO?> UpdateAsync(TUpdateDTO entity, int id);

        Task<TOutDTO?> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task RemoveRangeAsync(List<int> entities);
    }
}
