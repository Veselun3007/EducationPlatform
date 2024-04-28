using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Amazon.S3.Util.S3EventNotification;

namespace CourseService.Infrastructure.Repositories {
    public class CourseRepository : Repository<Course>, ICourseRepository {
        public CourseRepository(EducationPlatformContext context) : base(context) {}

        public override IQueryable<Course> FindBy(Expression<Func<Course, bool>> predicate) => DbSet.AsQueryable().Include(c => c.Courseusers).Where(predicate);
        public override IEnumerable<Course> FindEnumerable(Expression<Func<Course, bool>> predicate) => DbSet.Include(c => c.Courseusers).Where(predicate);
        public override Task<Course?> FirstOrDefaultAsync(Expression<Func<Course, bool>> predicate) => DbSet.Include(c => c.Courseusers).FirstOrDefaultAsync(predicate);

        //public override async Task<Course> AddAsync(Course entity) {
        //    do {
        //        entity.CourseLink = Guid.NewGuid().ToString();
        //    } while ((await DbSet.FirstOrDefaultAsync(c => c.CourseLink == entity.CourseLink)) != null);
        //    return (await DbSet.AddAsync(entity)).Entity;
        //}
        //public void GetAllCourses() {
        //    throw new NotImplementedException();
        //}
    }
}
