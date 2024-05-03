using EPChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPChat.Infrastructure.Contexts
{
    public class ChatDBContext : DbContext
    {
        public ChatDBContext() { }

        public ChatDBContext(DbContextOptions<ChatDBContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatMember> ChatMembers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageMedia> MessagesMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity => {
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

                entity.Property(e => e.ChatId)
                    .HasColumnName("chat_id");

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");

                entity.ToTable("users");

                entity.HasIndex(e => e.Email).IsUnique()
                .HasDatabaseName("users_user_email_key");

                entity.Property(e => e.Id)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);

                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .HasColumnName("user_name");

                entity.Property(e => e.Email)
                    .HasMaxLength(254)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserImage)
                    .HasColumnType("character varying")
                    .HasColumnName("user_image");
            });

            modelBuilder.Entity<ChatMember>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("chat_member_pkey");

                entity.ToTable("chat_members");

                entity.Property(e => e.Id)
                    .HasColumnName("chat_member_id");

                entity.Property(e => e.ChatId)
                    .HasColumnName("chat_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Course).WithMany(p => p.Members)
                    .HasForeignKey(d => d.ChatId)
                    .HasConstraintName("fk_members_course");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_members_user");

            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("message_pkey");

                entity.ToTable("messages");

                entity.Property(e => e.Id)
                    .HasColumnName("message_id");

                entity.Property(e => e.ChatId)
                    .HasColumnName("chat_id");

                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id");

                entity.Property(e => e.ReplyToMessageId)
                    .HasColumnName("reply_to_message_id");

                entity.Property(e => e.MessageText)
                    .HasMaxLength(500)
                    .HasColumnName("message_text");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.IsEdit)
                    .HasColumnName("is_edit");

                entity.Property(e => e.CreatedIn)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_in");

                entity.Property(e => e.EditedIn)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("edited_in");

                entity.HasOne(d => d.ReplyToMessage)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ReplyToMessageId)
                    .HasConstraintName("fk_answer");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId)
                    .HasConstraintName("fk_messages_course");

            });

            modelBuilder.Entity<MessageMedia>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("message_medias_pkey");

                entity.ToTable("message_medias");

                entity.Property(e => e.Id)
                    .HasColumnName("message_media_id");

                entity.Property(e => e.MessageId)
                    .HasColumnName("message_id");

                entity.Property(e => e.MediaLink)
                   .HasMaxLength(250)
                   .HasColumnName("media_link");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.AttachedMedias)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("fk_message_medias");
            });


            OnModelCreatingPartial(modelBuilder);
        }

    }
}
