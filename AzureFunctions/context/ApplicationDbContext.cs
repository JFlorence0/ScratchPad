using Microsoft.EntityFrameworkCore;

namespace http.context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Don't configure tables, just ensure EF Core can query them
            base.OnModelCreating(modelBuilder);
        }
    }
}
