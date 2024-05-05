using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories {
    public class CourseuserRepository : Repository<Courseuser> {
        public CourseuserRepository(EducationPlatformContext context) : base(context) {}

        public override IQueryable<Courseuser> FindBy(Expression<Func<Courseuser, bool>> predicate) => DbSet.AsQueryable().Include(c => c.User).Where(predicate);
        public override IEnumerable<Courseuser> FindEnumerable(Expression<Func<Courseuser, bool>> predicate) => DbSet.Include(c => c.User).Where(predicate);
        public override Task<Courseuser?> FirstOrDefaultAsync(Expression<Func<Courseuser, bool>> predicate) => DbSet.Include(c => c.User).FirstOrDefaultAsync(predicate);
    }
}
