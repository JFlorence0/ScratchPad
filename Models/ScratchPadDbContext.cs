using System.Globalization;
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
        public DbSet<Course> Courses { get; set; }
        
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
        public required InvestmentTheme InvestmentTheme { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CourseProvider { get; set; }

        // Using datetime over string for sorting.
        public DateTime DateCompleted { get; set; }

        public bool Completed { get; set; } = false;

        // Let myself provide a date or auto generate one
        public void MarkAsCompleted(DateTime? completedDate = null)
        {
            if (completedDate == null)
            {
                throw new ArgumentException("A completion date must be provided for past courses.");
            }

            Completed = true;
            DateCompleted = completedDate.Value;
        }
    }

    
}