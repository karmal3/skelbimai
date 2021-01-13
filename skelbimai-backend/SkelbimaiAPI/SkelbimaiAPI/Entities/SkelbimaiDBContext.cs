using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using SkelbimaiAPI.Helpers;

namespace SkelbimaiAPI.Entities
{
    public partial class SkelbimaiDBContext : DbContext
    {
        private readonly AppSettings _appSettings;

        public SkelbimaiDBContext()
        {
        }

        public SkelbimaiDBContext(DbContextOptions<SkelbimaiDBContext> options, IOptions<AppSettings> appSettings)
            : base(options)
        {
            _appSettings = appSettings.Value;
        }

        public virtual DbSet<Blocks> Blocks { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Commentsrating> Commentsrating { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Forumcategory> Forumcategory { get; set; }
        public virtual DbSet<Forumcomments> Forumcomments { get; set; }
        public virtual DbSet<Forumcommentsrating> Forumcommentsrating { get; set; }
        public virtual DbSet<Messagereceiver> Messagereceiver { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Messagesender> Messagesender { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Skelbimas> Skelbimas { get; set; }
        public virtual DbSet<Skelbimasrating> Skelbimasrating { get; set; }
        public virtual DbSet<Topic> Topic { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_appSettings.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Blocks>(entity =>
            {
                entity.ToTable("blocks", "skelbimaidb");

                entity.HasIndex(e => e.FkAdminId)
                    .HasName("fk_admin_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FkAdminId)
                    .HasColumnName("fk_admin_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkAdmin)
                    .WithMany(p => p.BlocksFkAdmin)
                    .HasForeignKey(d => d.FkAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blocks_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithOne(p => p.BlocksFkUser)
                    .HasForeignKey<Blocks>(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blocks_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category", "skelbimaidb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(5)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments", "skelbimaidb");

                entity.HasIndex(e => e.FkSkelbimasId)
                    .HasName("skelbimas_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DislikeCounter)
                    .HasColumnName("dislike_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FkSkelbimasId)
                    .HasColumnName("fk_skelbimas_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LikeCounter)
                    .HasColumnName("like_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkSkelbimas)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkSkelbimasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_ibfk_1");
            });

            modelBuilder.Entity<Commentsrating>(entity =>
            {
                entity.ToTable("commentsrating", "skelbimaidb");

                entity.HasIndex(e => e.FkCommentId)
                    .HasName("fk_comment_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Downvote)
                    .HasColumnName("downvote")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FkCommentId)
                    .HasColumnName("fk_comment_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Upvote)
                    .HasColumnName("upvote")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkComment)
                    .WithMany(p => p.Commentsrating)
                    .HasForeignKey(d => d.FkCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("commentsrating_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Commentsrating)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("commentsrating_ibfk_1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country", "skelbimaidb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Forumcategory>(entity =>
            {
                entity.ToTable("forumcategory", "skelbimaidb");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ViewCounter)
                    .HasColumnName("view_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Forumcategory)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forumcategory_ibfk_1");
            });

            modelBuilder.Entity<Forumcomments>(entity =>
            {
                entity.ToTable("forumcomments", "skelbimaidb");

                entity.HasIndex(e => e.FkTopicId)
                    .HasName("fk_topic_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DislikeCounter)
                    .HasColumnName("dislike_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FkTopicId)
                    .HasColumnName("fk_topic_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LikeCounter)
                    .HasColumnName("like_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkTopic)
                    .WithMany(p => p.Forumcomments)
                    .HasForeignKey(d => d.FkTopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forumcomments_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Forumcomments)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forumcomments_ibfk_1");
            });

            modelBuilder.Entity<Forumcommentsrating>(entity =>
            {
                entity.ToTable("forumcommentsrating", "skelbimaidb");

                entity.HasIndex(e => e.FkForumCommentId)
                    .HasName("fk_forum_comment_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Downvote)
                    .HasColumnName("downvote")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FkForumCommentId)
                    .HasColumnName("fk_forum_comment_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Upvote)
                    .HasColumnName("upvote")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkForumComment)
                    .WithMany(p => p.Forumcommentsrating)
                    .HasForeignKey(d => d.FkForumCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forumcommentsrating_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Forumcommentsrating)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("forumcommentsrating_ibfk_1");
            });

            modelBuilder.Entity<Messagereceiver>(entity =>
            {
                entity.ToTable("messagereceiver", "skelbimaidb");

                entity.HasIndex(e => e.MessageId)
                    .HasName("message_id");

                entity.HasIndex(e => e.ReceiverId)
                    .HasName("receiver_id");

                entity.HasIndex(e => e.SenderId)
                    .HasName("sender_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageId)
                    .HasColumnName("message_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReceiverId)
                    .HasColumnName("receiver_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Messagereceiver)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagereceiver_ibfk_3");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessagereceiverReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagereceiver_ibfk_2");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessagereceiverSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagereceiver_ibfk_1");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.ToTable("messages", "skelbimaidb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Messagesender>(entity =>
            {
                entity.ToTable("messagesender", "skelbimaidb");

                entity.HasIndex(e => e.MessageId)
                    .HasName("message_id");

                entity.HasIndex(e => e.ReceiverId)
                    .HasName("receiver_id");

                entity.HasIndex(e => e.SenderId)
                    .HasName("sender_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MessageId)
                    .HasColumnName("message_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReceiverId)
                    .HasColumnName("receiver_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Messagesender)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagesender_ibfk_3");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessagesenderReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagesender_ibfk_2");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessagesenderSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("messagesender_ibfk_1");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles", "skelbimaidb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Skelbimas>(entity =>
            {
                entity.ToTable("skelbimas", "skelbimaidb");

                entity.HasIndex(e => e.FkCategoryId)
                    .HasName("category_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkCategoryId)
                    .HasColumnName("fk_category_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(8,2)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ViewCounter)
                    .HasColumnName("view_counter")
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.Skelbimas)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("skelbimas_ibfk_1");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Skelbimas)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("skelbimas_ibfk_2");
            });

            modelBuilder.Entity<Skelbimasrating>(entity =>
            {
                entity.ToTable("skelbimasrating", "skelbimaidb");

                entity.HasIndex(e => e.FkSkelbimasId)
                    .HasName("fk_skelbimas_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FkSkelbimasId)
                    .HasColumnName("fk_skelbimas_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkSkelbimas)
                    .WithMany(p => p.Skelbimasrating)
                    .HasForeignKey(d => d.FkSkelbimasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("skelbimasrating_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Skelbimasrating)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("skelbimasrating_ibfk_1");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topic", "skelbimaidb");

                entity.HasIndex(e => e.FkForumcategoryId)
                    .HasName("fk_forumcategory_id");

                entity.HasIndex(e => e.FkUserId)
                    .HasName("fk_user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkForumcategoryId)
                    .HasColumnName("fk_forumcategory_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkUserId)
                    .HasColumnName("fk_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ViewCounter)
                    .HasColumnName("view_counter")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.FkForumcategory)
                    .WithMany(p => p.Topic)
                    .HasForeignKey(d => d.FkForumcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("topic_ibfk_2");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Topic)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("topic_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "skelbimaidb");

                entity.HasIndex(e => e.Email)
                    .HasName("Unique email")
                    .IsUnique();

                entity.HasIndex(e => e.FkCountryId)
                    .HasName("country_id");

                entity.HasIndex(e => e.Role)
                    .HasName("role");

                entity.HasIndex(e => e.Username)
                    .HasName("Unique username")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FkCountryId)
                    .HasColumnName("fk_country_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasColumnName("hash")
                    .HasColumnType("binary(64)");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.ProfilePicture)
                    .HasColumnName("profile_picture")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasColumnType("int(2)");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasColumnType("binary(128)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkCountry)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FkCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk_1");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk_2");
            });
        }
    }
}
