using System;
using System.Collections.Generic;
using CourseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext()
    {
    }

    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Courseuser> Courseusers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    //    optionsBuilder.UseLazyLoadingProxies();
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Course>(entity => {
            entity.HasKey(e => e.CourseId).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseDescription)
                .HasMaxLength(255)
                .HasColumnName("course_description");
            entity.Property(e => e.CourseLink)
                .HasMaxLength(20)
                .HasColumnName("course_link");
            entity.Property(e => e.CourseName)
                .HasMaxLength(128)
                .HasColumnName("course_name");
        });

        modelBuilder.Entity<Courseuser>(entity => {
            entity.HasKey(e => e.CourseuserId).HasName("course_users_pkey");

            entity.ToTable("course_users");

            entity.Property(e => e.CourseuserId).HasColumnName("course_user_id");
            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Courseusers)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_courseuser_course");

            entity.HasOne(d => d.User).WithMany(p => p.Courseusers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_courseuser_user");
        });

        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserEmail, "users_user_email_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");
            entity.Property(e => e.UserEmail)
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
