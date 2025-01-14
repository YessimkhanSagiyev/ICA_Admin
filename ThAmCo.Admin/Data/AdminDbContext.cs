using ThAmCo.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Admin.Data
{
    public class AdminDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
