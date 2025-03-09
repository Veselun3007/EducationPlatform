using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories.GenericRepositories;

namespace CourseContent.Infrastructure.Repositories
{
    public class MaterialfileRepository : EntityRepository<Materialfile, int>
    {
        public MaterialfileRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }

    public class AssignmentfileRepository : EntityRepository<Assignmentfile, int>
    {
        public AssignmentfileRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
