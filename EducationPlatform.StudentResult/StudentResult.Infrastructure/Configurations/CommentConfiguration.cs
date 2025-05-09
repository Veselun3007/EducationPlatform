using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentResult.Domain.Entities;

namespace StudentResult.Infrastructure.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(e => e.Id).HasName("comments_pkey");
            builder.ToTable("comments");

            builder.Property(e => e.Id).HasColumnName("comment_id");
            builder.Property(e => e.CommentDate).HasColumnName("comment_date");
            builder.Property(e => e.CommentText).HasColumnType("character varying").HasColumnName("comment_text");
            builder.Property(e => e.CourseUserId).ValueGeneratedOnAdd().HasColumnName("course_user_id");
            builder.Property(e => e.StudentAssignmentId).ValueGeneratedOnAdd().HasColumnName("studentassignment_id");

            builder.HasOne(d => d.CourseUser).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CourseUserId)
                .HasConstraintName("comments_course_user_id_fkey");
            builder.HasOne(d => d.Studentassignment).WithMany(p => p.Comments)
                .HasForeignKey(d => d.StudentAssignmentId)
                .HasConstraintName("comments_studentassignment_id_fkey");
        }
    }
}
