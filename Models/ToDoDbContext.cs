using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDoAPI.Models
{
    public partial class ToDoDbContext : DbContext
    {
        public ToDoDbContext()
        {
        }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activity { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Maprang22211;database=ToDo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.2.0-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasIndex(e => e.UserId, "FK_Activity_User");

                entity.Property(e => e.Id).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb3_thai_520_w2");

                entity.Property(e => e.UserId)
                    .HasMaxLength(13)
                    .UseCollation("utf8mb3_thai_520_w2");

                entity.Property(e => e.When).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(13)
                    .UseCollation("utf8mb3_thai_520_w2");

                entity.Property(e => e.Password)
                    .HasMaxLength(44)
                    .UseCollation("utf8mb3_thai_520_w2");

                entity.Property(e => e.Salt)
                    .HasMaxLength(24)
                    .UseCollation("utf8mb3_thai_520_w2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
