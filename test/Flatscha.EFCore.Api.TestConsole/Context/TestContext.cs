using Flatscha.EFCore.Api.TestConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace Flatscha.EFCore.Api.TestConsole.Context
{
    public partial class TestContext : DbContext
    {
        public TestContext()
        {
        }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_UserID");

                entity.ToTable("User", "Test");

                entity.HasIndex(e => e.UserName, "IX_User").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");
                entity.Property(e => e.FirstName).HasMaxLength(250);
                entity.Property(e => e.LastLogin).HasColumnType("datetime");
                entity.Property(e => e.LastName).HasMaxLength(250);
                entity.Property(e => e.ObjectChangedDateTime).HasColumnType("datetime");
                entity.Property(e => e.ObjectChangedUserId).HasColumnName("ObjectChangedUserID");
                entity.Property(e => e.ObjectCreatedDateTime).HasColumnType("datetime");
                entity.Property(e => e.ObjectCreatedUserId).HasColumnName("ObjectCreatedUserID");
                entity.Property(e => e.Password).HasMaxLength(250);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.UserName).HasMaxLength(50);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
