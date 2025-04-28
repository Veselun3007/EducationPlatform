using Chat.Domain.Entities;
using Chat.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Contexts
{
    public partial class EducationPlatformContext : DbContext
    {
        public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseUser> ChatMembers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageMedia> MessageMedias { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CourseUserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new MessageMediaConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
