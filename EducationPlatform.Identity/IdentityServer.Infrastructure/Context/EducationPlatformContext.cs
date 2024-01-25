using IdentityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext()
    {
    }

    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_user_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
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
