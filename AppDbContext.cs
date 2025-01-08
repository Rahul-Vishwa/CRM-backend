using Microsoft.EntityFrameworkCore;
using Artico.DbModels;

namespace Artico
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Users> users { get; set; }
        public DbSet<Customers> customers { get; set; }
    }
}
