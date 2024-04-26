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

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Assignmentfile> Assignmentfiles { get; set; }

    public virtual DbSet<Assignmentlink> Assignmentlinks { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Materialfile> Materialfiles { get; set; }

    public virtual DbSet<Materiallink> Materiallinks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.Property(e => e.Id).HasColumnName("assignment_id");

            entity.Property(e => e.AssignmentDatePublication)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("assignment_date_publication");

            entity.Property(e => e.AssignmentDeadline)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("assignment_deadline");

            entity.Property(e => e.AssignmentDescription)
            .HasColumnName("assignment_description");

            entity.Property(e => e.AssignmentName)
                .HasMaxLength(255)
                .HasColumnName("assignment_name");

            entity.Property(e => e.MaxMark)
                .HasColumnName("max_mark")
                .IsRequired(); 

            entity.Property(e => e.MinMark)
                .HasColumnName("min_mark")
                .IsRequired(); 

            entity.Property(e => e.IsRequired)
                .HasColumnName("is_required")
                .IsRequired();

            entity.Property(e => e.IsEdited)
                .HasColumnName("is_edited");                

            entity.Property(e => e.EditedTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("edited_time");

            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");

            entity.Property(e => e.TopicId)
                .ValueGeneratedOnAdd()
                .HasColumnName("topic_id");

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

            entity.ToTable("assignment_files");

            entity.Property(e => e.Id).HasColumnName("assignment_file_id");
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

        modelBuilder.Entity<Assignmentlink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assignmentlinks_pkey");

            entity.ToTable("assignment_links");

            entity.Property(e => e.Id).HasColumnName("assignment_link_id");
            entity.Property(e => e.AssignmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("assignment_id");

            entity.Property(e => e.AssignmentLink)
                .HasColumnType("character varying")
                .HasColumnName("assignment_link");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Assignmentlinks)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("fk_assignmentlinks_assignments");
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

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("topic_pkey");

            entity.ToTable("topics");

            entity.Property(e => e.Id).HasColumnName("topic_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("topic_name");

            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");

            entity.HasOne(e => e.Course).WithMany(p => p.Topics)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fk_topics_course");
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
                .HasColumnType("timestamp with time zone")
                .HasColumnName("material_date_publication");

            entity.Property(e => e.MaterialDescription)
                .HasColumnName("material_description");

            entity.Property(e => e.MaterialName)
                .HasMaxLength(255)
                .HasColumnName("material_name");

            entity.Property(e => e.IsEdited)
                .HasColumnName("is_edited");

            entity.Property(e => e.EditedTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("edited_time");

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

            entity.ToTable("material_files");

            entity.Property(e => e.Id).HasColumnName("material_file_id");
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

        modelBuilder.Entity<Materiallink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materialfiles_pkey");

            entity.ToTable("material_links");

            entity.Property(e => e.Id).HasColumnName("material_link_id");
            entity.Property(e => e.MaterialLink)
                .HasColumnType("character varying")
                .HasColumnName("material_link");
            entity.Property(e => e.MaterialId)
                .ValueGeneratedOnAdd()
                .HasColumnName("material_id");

            entity.HasOne(d => d.Material).WithMany(p => p.Materiallinks)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_materiallinks_materials");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
