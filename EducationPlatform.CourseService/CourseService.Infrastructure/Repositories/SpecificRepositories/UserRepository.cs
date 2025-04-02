using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Repositories.Generic;

namespace CourseService.Infrastructure.Repositories.SpecificRepositories
{
    internal class UserRepository : MinRepository<string, User>
    {
        public UserRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
