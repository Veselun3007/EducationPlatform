using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories {
    public class CourseRepository : Repository<Course> {
        public CourseRepository(EducationPlatformContext context) : base(context) {}

        public override IQueryable<Course> FindBy(Expression<Func<Course, bool>> predicate) => DbSet.AsQueryable().Include(c => c.Courseusers).Where(predicate);
        public override IEnumerable<Course> FindEnumerable(Expression<Func<Course, bool>> predicate) => DbSet.Include(c => c.Courseusers).Where(predicate);
        public override Task<Course?> FirstOrDefaultAsync(Expression<Func<Course, bool>> predicate) => DbSet.Include(c => c.Courseusers).FirstOrDefaultAsync(predicate);
    }
}
