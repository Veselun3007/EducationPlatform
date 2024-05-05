using Microsoft.EntityFrameworkCore;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Context;

public partial class EducationPlatformContext : DbContext {
    public EducationPlatformContext() {
    }

    public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
        : base(options) {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AttachedFile> AttachedFiles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CourseUser> CourseUsers { get; set; }

    public virtual DbSet<StudentAssignment> StudentAssignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=educationplatformdbinstance.c7cus0ucuuoc.us-east-1.rds.amazonaws.com;Database=EducationPlatformDB;User Id=postgres;Password=LNm57Iiv2km3ag58rm8U;");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Assignment>(entity => {
            entity.HasKey(e => e.AssignmentId).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.AssignmentDatePublication).HasColumnName("assignment_date_publication");
            entity.Property(e => e.AssignmentDeadline).HasColumnName("assignment_deadline");
            entity.Property(e => e.AssignmentDescription).HasColumnName("assignment_description");
            entity.Property(e => e.AssignmentName)
                .HasMaxLength(255)
                .HasColumnName("assignment_name");
            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");
            entity.Property(e => e.EditedTime).HasColumnName("edited_time");
            entity.Property(e => e.IsEdited).HasColumnName("is_edited");
            entity.Property(e => e.IsRequired).HasColumnName("is_required");
            entity.Property(e => e.MaxMark).HasColumnName("max_mark");
            entity.Property(e => e.MinMark).HasColumnName("min_mark");
            entity.Property(e => e.TopicId)
                .ValueGeneratedOnAdd()
                .HasColumnName("topic_id");
        });

        modelBuilder.Entity<AttachedFile>(entity => {
            entity.HasKey(e => e.AttachedFileId).HasName("attached_files_pkey");

            entity.ToTable("attached_files");

            entity.Property(e => e.AttachedFileId).HasColumnName("attached_file_id");
            entity.Property(e => e.AttachedFileName)
                .HasColumnType("character varying")
                .HasColumnName("attached_file");
            entity.Property(e => e.StudentassignmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("studentassignment_id");

            entity.HasOne(d => d.Studentassignment).WithMany(p => p.AttachedFiles)
                .HasForeignKey(d => d.StudentassignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_attached_files_student_assignments");
        });

        modelBuilder.Entity<Comment>(entity => {
            entity.HasKey(e => e.CommentId).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentDate).HasColumnName("comment_date");
            entity.Property(e => e.CommentText)
                .HasColumnType("character varying")
                .HasColumnName("comment_text");
            entity.Property(e => e.CourseUserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_user_id");
            entity.Property(e => e.StudentassignmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("studentassignment_id");

            entity.HasOne(d => d.CourseUser).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CourseUserId)
                .HasConstraintName("comments_course_user_id_fkey");

            entity.HasOne(d => d.Studentassignment).WithMany(p => p.Comments)
                .HasForeignKey(d => d.StudentassignmentId)
                .HasConstraintName("comments_studentassignment_id_fkey");
        });

        modelBuilder.Entity<CourseUser>(entity => {
            entity.HasKey(e => e.CourseUserId).HasName("course_users_pkey");

            entity.ToTable("course_users");

            entity.Property(e => e.CourseUserId).HasColumnName("course_user_id");
            entity.Property(e => e.CourseId)
                .ValueGeneratedOnAdd()
                .HasColumnName("course_id");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.CourseUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_courseuser_user");
        });

        modelBuilder.Entity<StudentAssignment>(entity => {
            entity.HasKey(e => e.StudentassignmentId).HasName("student_assignments_pkey");

            entity.ToTable("student_assignments");

            entity.Property(e => e.StudentassignmentId).HasColumnName("studentassignment_id");
            entity.Property(e => e.AssignmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("assignment_id");
            entity.Property(e => e.CurrentMark).HasColumnName("current_mark");
            entity.Property(e => e.IsDone).HasColumnName("is_done");
            entity.Property(e => e.StudentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("student_id");
            entity.Property(e => e.SubmissionDate).HasColumnName("submission_date");

            entity.HasOne(d => d.Assignment).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("fk_studentassignment_assignments");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAssignments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("fk_studentassignment_course_user");
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
