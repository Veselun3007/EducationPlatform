using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Configurations
{
    internal class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(e => e.Id).HasName("assignments_pkey");
            builder.ToTable("assignments");

            builder.Property(e => e.Id).HasColumnName("assignment_id");
            builder.Property(e => e.AssignmentDatePublication).HasColumnType("timestamp with time zone").HasColumnName("assignment_date_publication");
            builder.Property(e => e.AssignmentDeadline).HasColumnType("timestamp with time zone").HasColumnName("assignment_deadline");
            builder.Property(e => e.AssignmentDescription).HasColumnName("assignment_description");
            builder.Property(e => e.AssignmentName).HasMaxLength(255).HasColumnName("assignment_name");
            builder.Property(e => e.MaxMark).HasColumnName("max_mark").IsRequired();
            builder.Property(e => e.MinMark).HasColumnName("min_mark").IsRequired();
            builder.Property(e => e.IsRequired).HasColumnName("is_required").IsRequired();
            builder.Property(e => e.IsEdited).HasColumnName("is_edited");
            builder.Property(e => e.EditedTime).HasColumnType("timestamp with time zone").HasColumnName("edited_time");
            builder.Property(e => e.CourseId).ValueGeneratedOnAdd().HasColumnName("course_id");
            builder.Property(e => e.TopicId).HasColumnName("topic_id");
        }
    }
}
