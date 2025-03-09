using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext() { }
    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options) : base(options) { }

    public virtual DbSet<Topic> Topics { get; set; }
    public virtual DbSet<Assignment> Assignments { get; set; }
    public virtual DbSet<Assignmentfile> Assignmentfiles { get; set; }
    public virtual DbSet<Assignmentlink> Assignmentlinks { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<Materialfile> Materialfiles { get; set; }
    public virtual DbSet<Materiallink> Materiallinks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AssignmentConfiguration().Configure(modelBuilder.Entity<Assignment>());
        new AssignmentfileConfiguration().Configure(modelBuilder.Entity<Assignmentfile>());
        new AssignmentlinkConfiguration().Configure(modelBuilder.Entity<Assignmentlink>());
        new MaterialConfiguration().Configure(modelBuilder.Entity<Material>());
        new MaterialfileConfiguration().Configure(modelBuilder.Entity<Materialfile>());
        new MateriallinkConfiguration().Configure(modelBuilder.Entity<Materiallink>());
        new TopicConfiguration().Configure(modelBuilder.Entity<Topic>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
