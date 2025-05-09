using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Context;
using StudentResult.Infrastructure.Repositories.Generic;

namespace StudentResult.Infrastructure.Repositories
{
    internal class CommentRepository : Repository<int, Comment>
    {
        public CommentRepository(EducationPlatformContext context) : base(context) { }
    }

    internal class CourseuserRepository : Repository<int, CourseUser>
    {
        public CourseuserRepository(EducationPlatformContext context) : base(context) { }
    }

    internal class StudentAssignmentRepository : Repository<int, StudentAssignment>
    {
        public StudentAssignmentRepository(EducationPlatformContext context) : base(context) { }
    }

    internal class AttachedFileRepository : Repository<int, AttachedFile>
    {
        public AttachedFileRepository(EducationPlatformContext dbContext) : base(dbContext) { }
    }
}
