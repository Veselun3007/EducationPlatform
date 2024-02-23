using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Context
{
    public partial class EducationPlatformContext : DbContext
    {
        public EducationPlatformContext() { }

        public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");

                entity.ToTable("users");

                entity.HasIndex(e => e.Email).IsUnique()
                .HasDatabaseName("users_user_email_key");

                entity.Property(e => e.Id).HasColumnName("user_id");
                entity.Property(e => e.Email)
                    .HasMaxLength(254)
                    .HasColumnName("user_email");
                entity.Property(e => e.UserImage)
                    .HasColumnType("character varying")
                    .HasColumnName("user_image");
                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .HasColumnName("user_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}