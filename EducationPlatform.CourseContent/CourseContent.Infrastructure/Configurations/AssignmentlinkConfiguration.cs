using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class AssignmentlinkConfiguration : IEntityTypeConfiguration<Assignmentlink>
    {
        public void Configure(EntityTypeBuilder<Assignmentlink> builder)
        {
            builder.HasKey(e => e.Id).HasName("assignmentlinks_pkey");
            builder.ToTable("assignment_links");

            builder.Property(e => e.Id).HasColumnName("assignment_link_id");
            builder.Property(e => e.AssignmentId).ValueGeneratedOnAdd().HasColumnName("assignment_id");
            builder.Property(e => e.AssignmentLink).HasColumnType("character varying").HasColumnName("assignment_link");

            builder.HasOne(d => d.Assignment).WithMany(p => p.Assignmentlinks).HasForeignKey(d => d.AssignmentId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("fk_assignmentlinks_assignments");
        }
    }
}
