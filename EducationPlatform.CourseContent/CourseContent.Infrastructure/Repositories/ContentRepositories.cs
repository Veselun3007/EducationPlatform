using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;
using CourseContent.Infrastructure.Repositories.GenericRepositories;

namespace CourseContent.Infrastructure.Repositories
{
    public class AssignmentRepository : ContentRepository<Assignment, int>
    {
        public AssignmentRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }

    public class MaterialRepository : ContentRepository<Material, int>
    {
        public MaterialRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }

    public class TopicRepository : ContentRepository<Topic, int>
    {
        public TopicRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
