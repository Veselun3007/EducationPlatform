using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Context;
using System.Linq.Expressions;

namespace StudentResult.Infrastructure.Repositories {
    public class CourseuserRepository : Repository<CourseUser> {
        public CourseuserRepository(EducationPlatformContext context) : base(context) { }

        public override IQueryable<CourseUser> FindBy(Expression<Func<CourseUser, bool>> predicate) => DbSet.AsQueryable().Include(c => c.User).Where(predicate);
        public override IEnumerable<CourseUser> FindEnumerable(Expression<Func<CourseUser, bool>> predicate) => DbSet.Include(c => c.User).Where(predicate);
        public override Task<CourseUser?> FirstOrDefaultAsync(Expression<Func<CourseUser, bool>> predicate) => DbSet.Include(c => c.User).FirstOrDefaultAsync(predicate);
    }
}
