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

        public DbSet<InvestmentIdea> InvestmentIdeas { get; set; }
        
    }

    // Define the InvestmentTheme model
    public class InvestmentTheme
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property for related InvestmentIdeas
        public List<InvestmentIdea> InvestmentIdeas { get; set; } = new();
    }

    // Define the InvestmentIdea model
    public class InvestmentIdea
    {
        public int Id { get; set; } // Primary Key
        public required string Name { get; set; }
        public string Ticker { get; set; } = "N/A";
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign key to link to InvestmentTheme
        public int InvestmentThemeId { get; set; }
        public InvestmentTheme InvestmentTheme { get; set; }
    }
}