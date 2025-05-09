using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Configurations
{
    internal class StudentAssignmentConfiguration : IEntityTypeConfiguration<StudentAssignment>
    {
        public void Configure(EntityTypeBuilder<StudentAssignment> builder)
        {
            builder.HasKey(e => e.Id).HasName("student_assignments_pkey");
            builder.ToTable("student_assignments");

            builder.Property(e => e.Id).HasColumnName("studentassignment_id");
            builder.Property(e => e.AssignmentId).ValueGeneratedOnAdd().HasColumnName("assignment_id");
            builder.Property(e => e.CurrentMark).HasColumnName("current_mark");
            builder.Property(e => e.IsDone).HasColumnName("is_done");
            builder.Property(e => e.StudentId) .ValueGeneratedOnAdd().HasColumnName("student_id");
            builder.Property(e => e.SubmissionDate).HasColumnName("submission_date");

            builder.HasOne(d => d.Assignment).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("fk_studentassignment_assignments");
            builder.HasOne(d => d.Student).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("fk_studentassignment_course_user");
        }
    }
}
