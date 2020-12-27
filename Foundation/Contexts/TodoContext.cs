using API.Foundation.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Foundation.Contexts
{
    public class TodoContext : DbContext, ITodoContext
    {
        private string _connectionString;
        private string _migrationAssemblyName;

        public TodoContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasKey(x => x.Id);
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
