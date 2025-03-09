using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories.GenericRepositories;

namespace CourseContent.Infrastructure.Repositories
{
    public class MateriallinkRepository : MinRepository<Materiallink, int>
    {
        public MateriallinkRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }

    public class AssignmentlinkRepository : MinRepository<Assignmentlink, int>
    {
        public AssignmentlinkRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
