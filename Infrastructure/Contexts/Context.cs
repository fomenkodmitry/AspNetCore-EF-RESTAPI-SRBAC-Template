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
            
            modelBuilder.Entity<SrbacRolePermissionModel>()
                .HasIndex(b => new {b.Role, b.Permission})
                .IsUnique();

        }
    }
}