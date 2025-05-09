using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Configurations
{
    internal class CourseuserConfiguration : IEntityTypeConfiguration<CourseUser>
    {
        public void Configure(EntityTypeBuilder<CourseUser> builder)
        {
            builder.HasKey(e => e.Id).HasName("course_users_pkey");
            builder.ToTable("course_user");

            builder.Property(e => e.Id).HasColumnName("course_user_id");
            builder.Property(e => e.CourseId).ValueGeneratedOnAdd().HasColumnName("course_id");
            builder.Property(e => e.Role).HasColumnName("role");
            builder.Property(e => e.UserId).HasMaxLength(36).HasColumnName("user_id");
         
            builder.HasOne(d => d.User).WithMany(p => p.CourseUsers).HasForeignKey(d => d.UserId).HasConstraintName("fk_courseuser_user");
        }
    }
}
