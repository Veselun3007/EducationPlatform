using CourseService.Domain.Entities;
using CourseService.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext() { }
    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options) : base(options) { }

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Courseuser> Courseusers { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new CourseConfiguration().Configure(modelBuilder.Entity<Course>());
        new CourseuserConfiguration().Configure(modelBuilder.Entity<Courseuser>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
