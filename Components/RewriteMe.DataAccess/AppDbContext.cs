using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.Entities;
using RewriteMe.DataAccess.EntitiesConfiguration;

namespace RewriteMe.DataAccess
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString, DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _connectionString = connectionString;
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<FileItemEntity> FileItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, providerOptions => providerOptions.CommandTimeout(60));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FileItemEntityConfiguration());
        }
    }
}
