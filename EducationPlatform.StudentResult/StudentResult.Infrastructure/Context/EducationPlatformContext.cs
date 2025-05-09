using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;
using StudentResult.Infrastructure.Configurations;

namespace StudentResult.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options) : base(options) { }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AttachedFile> AttachedFiles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CourseUser> CourseUsers { get; set; }

    public virtual DbSet<StudentAssignment> StudentAssignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AssignmentConfiguration().Configure(modelBuilder.Entity<Assignment>());
        new AttachedFileConfiguration().Configure(modelBuilder.Entity<AttachedFile>());
        new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());
        new CourseuserConfiguration().Configure(modelBuilder.Entity<CourseUser>());
        new StudentAssignmentConfiguration().Configure(modelBuilder.Entity<StudentAssignment>());
        new UserConfiguration().Configure(modelBuilder.Entity<User>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
