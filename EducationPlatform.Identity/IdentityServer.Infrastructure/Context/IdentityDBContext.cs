using IdentityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Context
{
    public partial class IdentityDBContext : DbContext
    {

        public IdentityDBContext() { }

        public IdentityDBContext(DbContextOptions<IdentityDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(u => u.Id).HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(u => u.UserName)
                .HasMaxLength(250)
                .HasColumnName("user_name");

                entity.HasIndex(u => u.Email, "users_user_email_key").IsUnique();
                entity.Property(u => u.Email)
                .HasMaxLength(254)
                .HasColumnName("email");

                entity.Property(u => u.Password)
                .HasMaxLength(64)
                .HasColumnName("password");

                entity.Property(u => u.Salt)
                .HasMaxLength(64)
                .HasColumnName("salt");

            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(u => u.Id).HasName("tokens_pkey");

                entity.ToTable("tokens");

                entity.Property(u => u.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("user_id");

                entity.HasIndex(u => u.RefreshToken, "tokens_token_refreshtoken_key").IsUnique();
                entity.Property(u => u.RefreshToken)
                .HasMaxLength(36)
                .HasColumnName("refresh_token");

                entity.Property(u => u.RefreshTokenValidUntil)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_valid_until");

                entity.HasOne(d => d.AppUser).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_tokens_users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
