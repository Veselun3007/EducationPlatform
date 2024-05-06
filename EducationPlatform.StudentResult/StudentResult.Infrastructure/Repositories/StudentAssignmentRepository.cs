using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Context;
using System.Linq.Expressions;

namespace StudentResult.Infrastructure.Repositories {
    public class StudentAssignmentRepository : Repository<StudentAssignment> {
        public StudentAssignmentRepository(EducationPlatformContext context) : base(context) { }

        public override IQueryable<StudentAssignment> FindBy(Expression<Func<StudentAssignment, bool>> predicate) => DbSet.AsQueryable().Include(sa => sa.Comments).Include(sa => sa.AttachedFiles).Where(predicate);
        public override IEnumerable<StudentAssignment> FindEnumerable(Expression<Func<StudentAssignment, bool>> predicate) => DbSet.Include(sa => sa.Comments).Include(sa => sa.AttachedFiles).Where(predicate);
        public override Task<StudentAssignment?> FirstOrDefaultAsync(Expression<Func<StudentAssignment, bool>> predicate) => DbSet.Include(sa => sa.Comments).Include(sa => sa.AttachedFiles).Include(sa => sa.Assignment).FirstOrDefaultAsync(predicate);
    }
}
