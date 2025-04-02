using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Repositories.Generic;

namespace CourseService.Infrastructure.Repositories.SpecificRepositories
{
    internal class CourseuserRepository : ExtendedRepository<int, Courseuser>
    {
        public CourseuserRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
