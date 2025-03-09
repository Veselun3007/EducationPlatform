using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(e => e.Id).HasName("materials_pkey");
            builder.ToTable("materials");

            builder.Property(e => e.Id).HasColumnName("material_id");
            builder.Property(e => e.CourseId).ValueGeneratedOnAdd().HasColumnName("course_id");
            builder.Property(e => e.TopicId).HasColumnName("topic_id");
            builder.Property(e => e.MaterialDatePublication).HasColumnType("timestamp with time zone").HasColumnName("material_date_publication");
            builder.Property(e => e.MaterialDescription).HasColumnName("material_description");
            builder.Property(e => e.MaterialName).HasMaxLength(255).HasColumnName("material_name");
            builder.Property(e => e.IsEdited).HasColumnName("is_edited");
            builder.Property(e => e.EditedTime).HasColumnType("timestamp with time zone").HasColumnName("edited_time");

            builder.HasOne(d => d.Course).WithMany(p => p.Materials).HasForeignKey(d => d.CourseId).HasConstraintName("fk_materials_course");
            builder.HasOne(d => d.Topic).WithMany(t => t.Materials).HasForeignKey(d => d.TopicId).HasConstraintName("fk_materials_topics");
        }
    }
}
