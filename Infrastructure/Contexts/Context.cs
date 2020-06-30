using Domain.Audit;
using Domain.Authenticate;
using Domain.Code;
using Domain.FileStorage;
using Domain.Srbac;
using Domain.Token;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public sealed class Context : DbContext
    {
        public Context (DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<CodeModel> Codes { get; set; }
        
        public DbSet<SrbacRolePermissionModel> SrbacRolePermissions { get; set; }
        public DbSet<AuditModel> Audits { get; set; }
        public DbSet<FileModel> Files { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(b => b.Email)
                .IsUnique();
            modelBuilder.Entity<UserModel>()
                .HasIndex(b => b.Phone)
                .IsUnique();
            modelBuilder.Entity<UserModel>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("NOW()");
            modelBuilder.Entity<UserModel>()
                .Property(b => b.IsActive)
                .HasDefaultValueSql("true");
            modelBuilder.Entity<UserModel>()
                .Property(b => b.NamePatronymic)
                .HasDefaultValueSql("null");
            modelBuilder.Entity<UserModel>()
                .Property(b => b.Phone)
                .HasDefaultValueSql("null");

            modelBuilder.Entity<TokenModel>()
                .HasIndex(b => b.UserId);
            modelBuilder.Entity<TokenModel>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("NOW()");
            modelBuilder.Entity<TokenModel>()
                .Property(b => b.UserAgent)
                .HasDefaultValueSql("null");
            modelBuilder.Entity<TokenModel>()
                .HasOne<UserModel>(g => g.User)
                .WithMany(s => s.Tokens)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CodeModel>()
                .HasIndex(b => b.Code);
            
            modelBuilder.Entity<SrbacRolePermissionModel>()
                .HasIndex(b => new {b.Role, b.Permission})
                .IsUnique();

            modelBuilder.Entity<AuditModel>()
                .HasIndex(b => b.ObjectId);
            modelBuilder.Entity<AuditModel>()
                .HasIndex(b => b.Roles);
            modelBuilder.Entity<AuditModel>()
                .HasIndex(b => b.OperationType);
            modelBuilder.Entity<AuditModel>()
                .HasIndex(b => b.Status);
            
            modelBuilder.Entity<FileModel>()
                .HasIndex(b => b.EntityId);
        }
    }
}