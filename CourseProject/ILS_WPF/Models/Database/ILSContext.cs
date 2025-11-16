using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.Models.Database
{
    public class ILSContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public ILSContext()
            => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ILSConnectionString"));
    }
}
