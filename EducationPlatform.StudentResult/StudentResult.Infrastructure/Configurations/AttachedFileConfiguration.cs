using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Configurations
{
    internal class AttachedFileConfiguration : IEntityTypeConfiguration<AttachedFile>
    {
        public void Configure(EntityTypeBuilder<AttachedFile> builder)
        {
            builder.HasKey(e => e.Id).HasName("attached_files_pkey");
            builder.ToTable("attached_files");

            builder.Property(e => e.Id).HasColumnName("attached_file_id");
            builder.Property(e => e.AttachedFileName).HasColumnType("character varying").HasColumnName("attached_file");
            builder.Property(e => e.StudentassignmentId).ValueGeneratedOnAdd().HasColumnName("studentassignment_id");

            builder.HasOne(d => d.Studentassignment).WithMany(p => p.AttachedFiles)
                .HasForeignKey(d => d.StudentassignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_attached_files_student_assignments");
        }
    }
}
