using CourseService.Domain.Entities;
using CourseService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Infrastructure.Repositories {
    public class CourseuserRepository : Repository<Courseuser> {
        public CourseuserRepository(DbContext context) : base(context) {}
    }
}
