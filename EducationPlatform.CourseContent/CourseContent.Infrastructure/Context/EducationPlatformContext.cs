using CourseContent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseContent.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext
{
    public EducationPlatformContext()
    {
    }

    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Assignmentfile> Assignmentfiles { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Materialfile> Materialfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.Property(e => e.Id).HasColumnName("assignment_id");
            entity.Property(e => e.AssignmentDatePublication)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignment_date_publication");
            entity.Property(e => e.AssignmentDeadline)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignment_deadline");
            entity.Property(e => e.AssignmentDescription).HasColumnName("assignment_description");
            entity.Property(e => e.AssignmentName)
                .HasMaxLength(255)
                .HasColumnName("assignment_name");
            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_assignments_course");

            entity.HasOne(d => d.Topic).WithMany(t => t.Assignments)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fk_assignments_topics");


        });

        modelBuilder.Entity<Assignmentfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assignmentfiles_pkey");

            entity.ToTable("assignmentfiles");

            entity.Property(e => e.Id).HasColumnName("assignment_attachedfile_id");
            entity.Property(e => e.AssignmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("assignment_id");
            entity.Property(e => e.AssignmentFile)
                .HasColumnType("character varying")
                .HasColumnName("assignment_file");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Assignmentfiles)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assignmentfiles_assignments");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("course_id");
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

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materials_pkey");

            entity.ToTable("materials");

            entity.Property(e => e.Id).HasColumnName("material_id");
            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");
            entity.Property(e => e.MaterialDatePublication)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("material_date_publication");
            entity.Property(e => e.MaterialDescription).HasColumnName("material_description");
            entity.Property(e => e.MaterialName)
                .HasMaxLength(255)
                .HasColumnName("material_name");

            entity.HasOne(d => d.Course).WithMany(p => p.Materials)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_materials_course");

            entity.HasOne(d => d.Topic).WithMany(t => t.Materials)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fk_materials_topics");
        });

        modelBuilder.Entity<Materialfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialfiles_pkey");

            entity.ToTable("materialfiles");

            entity.Property(e => e.Id).HasColumnName("material_attachedfile_id");
            entity.Property(e => e.MaterialFile)
                .HasColumnType("character varying")
                .HasColumnName("material_file");
            entity.Property(e => e.MaterialId)
                .ValueGeneratedOnAdd()
                .HasColumnName("material_id");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialfiles)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_materialfiles_materials");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
