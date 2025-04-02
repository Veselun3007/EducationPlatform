using CourseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Infrastructure.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(e => e.Id).HasName("courses_pkey");
            builder.ToTable("courses");

            builder.Property(e => e.Id).HasColumnName("course_id");
            builder.Property(e => e.CourseDescription).HasMaxLength(255).HasColumnName("course_description");
            builder.Property(e => e.CourseLink).HasMaxLength(170).HasColumnName("course_link");
            builder.Property(e => e.CourseName).HasMaxLength(128).HasColumnName("course_name");
        }
    }
}
