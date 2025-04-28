using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Configurations
{
    internal class MessageMediaConfiguration : IEntityTypeConfiguration<MessageMedia>
    {
        public void Configure(EntityTypeBuilder<MessageMedia> builder)
        {
            builder.HasKey(e => e.Id).HasName("message_medias_pkey");
            builder.ToTable("message_medias");

            builder.Property(e => e.Id).HasColumnName("message_media_id");
            builder.Property(e => e.MessageId).HasColumnName("message_id");
            builder.Property(e => e.MediaLink).HasMaxLength(250).HasColumnName("media_link");

            builder.HasOne(d => d.Message).WithMany(p => p.AttachedMedias).HasForeignKey(d => d.MessageId).HasConstraintName("fk_message_medias");
        }
    }
}
