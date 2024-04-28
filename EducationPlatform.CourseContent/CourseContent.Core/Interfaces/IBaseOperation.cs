using CSharpFunctionalExtensions;

namespace CourseContent.Core.Interfaces
{
    public interface IBaseOperation<O, Error, I, U>
    {
        Task<Result<O, Error>> CreateAsync(I entity);

        Task<Result<O, Error>> UpdateAsync(U entity, int id);

        Task<Result<O, Error>> GetByIdAsync(int id);

        Task<Result<string, Error>> DeleteAsync(int id);

        Task<IEnumerable<O>> GetAllByCourseAsync(int courseId);
    }
}
