using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseContent.Infrastructure.Configurations
{
    internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(e => e.Id).HasName("topic_pkey");
            builder.ToTable("topics");

            builder.Property(e => e.Id).HasColumnName("topic_id");
            builder.Property(e => e.CourseId).ValueGeneratedOnAdd().HasColumnName("course_id");
            builder.Property(e => e.Title).HasMaxLength(255).HasColumnName("topic_name");

            builder.HasOne(e => e.Course).WithMany(p => p.Topics).HasForeignKey(d => d.CourseId).HasConstraintName("fk_topics_course");
        }
    }
}
