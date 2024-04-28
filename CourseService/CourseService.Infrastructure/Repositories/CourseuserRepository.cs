using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories {
    public class CourseuserRepository : Repository<Courseuser>, ICourseRepository {
        public CourseuserRepository(EducationPlatformContext context) : base(context) {}

        public override IQueryable<Courseuser> FindBy(Expression<Func<Courseuser, bool>> predicate) => DbSet.AsQueryable().Include(c => c.User).Where(predicate);
        public override IEnumerable<Courseuser> FindEnumerable(Expression<Func<Courseuser, bool>> predicate) => DbSet.Include(c => c.User).Where(predicate);
        public override Task<Courseuser?> FirstOrDefaultAsync(Expression<Func<Courseuser, bool>> predicate) => DbSet.Include(c => c.User).FirstOrDefaultAsync(predicate);
    }
}
