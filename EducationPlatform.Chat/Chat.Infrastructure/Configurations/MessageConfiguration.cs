using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Configurations
{
    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(e => e.Id).HasName("messages_pkey");
            builder.ToTable("messages");

            builder.Property(e => e.Id) .HasColumnName("message_id");
            builder.Property(e => e.CourseId).HasColumnName("course_id");
            builder.Property(e => e.CreatorId).HasColumnName("creator_id");
            builder.Property(e => e.MessageText).HasMaxLength(500).HasColumnName("message_text");
            builder.Property(e => e.IsDeleted) .HasColumnName("is_deleted");
            builder.Property(e => e.IsEdit).HasColumnName("is_edit");
            builder.Property(e => e.CreatedIn).HasColumnType("timestamp with time zone").HasColumnName("created_in");
            builder.Property(e => e.EditedIn).HasColumnType("timestamp with time zone").HasColumnName("edited_in");

            builder.HasOne(d => d.Course).WithMany(p => p.Messages).HasForeignKey(d => d.CourseId).HasConstraintName("fk_messages_course");
            builder.HasOne(d => d.CourseUser).WithMany(p => p.Messages) .HasForeignKey(d => d.CreatorId).HasConstraintName("fk_messages_user");
        }
    }
}
