using CSharpFunctionalExtensions;

namespace CourseContent.Core.Interfaces
{
    public interface IBaseOperation<T, Error, E>
    {
        Task<Result<T, Error>> CreateAsync(E entity);

        Task<Result<T, Error>> UpdateAsync(E entity, int id);

        Task<Result<T, Error>> GetByIdAsync(int id);

        Task<Result<string, Error>> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAllByCourseAsync(int courseId);
    }
}
