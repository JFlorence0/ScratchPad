using Microsoft.EntityFrameworkCore;

namespace ScratchPad.Models
{
    public class ScratchPadDbContext : DbContext
    {
        public ScratchPadDbContext(DbContextOptions<ScratchPadDbContext> options) : base(options)
        {
        }

        // DbSet represents a table in the database
        public DbSet<InvestmentTheme> InvestmentThemes { get; set; }
        
    }

    // Define the InvestmentTheme model
    public class InvestmentTheme
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Column for the theme name
        public string Description { get; set; } // Column for the theme description
        public DateTime CreatedDate { get; set; } // Column for when the theme was created
    }
}