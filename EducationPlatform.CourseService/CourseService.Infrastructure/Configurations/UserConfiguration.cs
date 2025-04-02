using CourseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id).HasName("users_pkey");
            builder.ToTable("users");
            builder.HasIndex(e => e.UserEmail, "users_user_email_key").IsUnique();

            builder.Property(e => e.Id).HasMaxLength(36).HasColumnName("user_id");
            builder.Property(e => e.UserEmail).HasMaxLength(254).HasColumnName("user_email");
            builder.Property(e => e.UserImage).HasColumnType("character varying").HasColumnName("user_image");
            builder.Property(e => e.UserName).HasMaxLength(250).HasColumnName("user_name");
        }
    }
}
