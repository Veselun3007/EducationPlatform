using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Context;
using StudentResult.Infrastructure.Repositories.Generic;

namespace StudentResult.Infrastructure.Repositories
{
    internal class UserRepository : MinRepository<string, User>
    {
        public UserRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }

    internal class AssignmentRepository : MinRepository<int, Assignment>
    {
        public AssignmentRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
