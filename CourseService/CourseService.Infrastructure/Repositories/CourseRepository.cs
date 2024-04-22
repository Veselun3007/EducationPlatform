using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Infrastructure.Repositories {
    public class CourseRepository : Repository<Course>, ICourseRepository {
        public CourseRepository(EducationPlatformContext context) : base(context) {}

        public override async Task<Course> AddAsync(Course entity) {
            do {
                entity.CourseLink = Guid.NewGuid().ToString();
            } while ((await DbSet.FirstOrDefaultAsync(c => c.CourseLink == entity.CourseLink)) != null);
            return (await DbSet.AddAsync(entity)).Entity;
        }

        public void GetAllCourses() {
            throw new NotImplementedException();
        }
    }
}
