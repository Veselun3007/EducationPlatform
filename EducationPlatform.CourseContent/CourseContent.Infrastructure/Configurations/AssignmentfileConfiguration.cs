using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class AssignmentfileConfiguration : IEntityTypeConfiguration<Assignmentfile>
    {
        public void Configure(EntityTypeBuilder<Assignmentfile> builder)
        {
            builder.HasKey(e => e.Id).HasName("assignmentfiles_pkey");
            builder.ToTable("assignment_files");

            builder.Property(e => e.Id).HasColumnName("assignment_file_id");
            builder.Property(e => e.AssignmentId).ValueGeneratedOnAdd().HasColumnName("assignment_id");
            builder.Property(e => e.AssignmentFile).HasColumnType("character varying").HasColumnName("assignment_file");

            builder.HasOne(d => d.Assignment).WithMany(p => p.Assignmentfiles).HasForeignKey(d => d.AssignmentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_assignmentfiles_assignments");
        }
    }
}
