using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Repositories.Generic;

namespace CourseService.Infrastructure.Repositories.SpecificRepositories
{
    internal class CourseRepository : Repository<int, Course>
    {
        public CourseRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
