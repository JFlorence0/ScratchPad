using System.Collections.Generic;               // For IAsyncEnumerable<T>
using System.Runtime.CompilerServices;          // For [EnumeratorCancellation]
using System.Threading;                         // For CancellationToken
using System.Threading.Tasks;                   // For async methods and ValueTask
using MySqlConnector;                           // For MySQL database connectivity
using ScratchPad.Models;                        // Assumes InvestmentIdea & InvestmentTheme are defined here

namespace ScratchPad.AzureFunctions.Repositories
{
    // The repository interface defines the operations available for Investment Ideas.
    public interface IInvestmentIdeaRepository
    {
        // Returns all investment ideas as an asynchronous stream.
        IAsyncEnumerable<InvestmentIdea> GetInvestmentIdeasAsync(CancellationToken cancellationToken = default);

        // Returns a single investment idea by its id, or null if not found.
        ValueTask<InvestmentIdea?> GetInvestmentIdeaAsync(int id, CancellationToken cancellationToken = default);
    }

    // A basic SQL implementation of IInvestmentIdeaRepository.
    public class InvestmentIdeaRepository : IInvestmentIdeaRepository
    {
        private readonly string _connectionString;

        // The constructor accepts a connection string used to connect to the MySQL database.
        public InvestmentIdeaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Retrieves all investment ideas as a stream.
        public async IAsyncEnumerable<InvestmentIdea> GetInvestmentIdeasAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // SQL query to fetch investment ideas along with their associated theme.
            string sql = @"
            SELECT 
                i.Id AS InvestmentId, 
                i.Name AS InvestmentName, 
                i.Ticker, 
                i.Description AS InvestmentDescription, 
                i.CreatedAt AS InvestmentCreatedAt, 
                t.Id AS ThemeId,
                t.Name AS ThemeName, 
                t.Description AS ThemeDescription, 
                t.CreatedDate AS ThemeCreatedDate
            FROM investmentideas i
            JOIN investmentthemes t ON i.InvestmentThemeId = t.Id";

            // Open a new connection using the provided connection string.
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Create the SQL command using the query.
            await using var command = new MySqlCommand(sql, connection);

            // Execute the command and get a reader for the result set.
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Asynchronously iterate through each row in the result set.
            while (await reader.ReadAsync(cancellationToken))
            {
                // Map each row to an InvestmentIdea object.
                var idea = new InvestmentIdea
                {
                    Id = reader.GetInt32("InvestmentId"),
                    // The required properties are set using the aliased column names.
                    Name = reader.GetString("InvestmentName"),
                    Description = reader.GetString("InvestmentDescription"),
                    Ticker = reader.GetString("Ticker"),
                    CreatedAt = reader.GetDateTime("InvestmentCreatedAt"),
                    // Build the nested InvestmentTheme object.
                    InvestmentTheme = new InvestmentTheme
                    {
                        Id = reader.GetInt32("ThemeId"),
                        Name = reader.GetString("ThemeName"),
                        Description = reader.GetString("ThemeDescription"),
                        CreatedDate = reader.GetDateTime("ThemeCreatedDate")
                    }
                };

                // Yield return streams the object back to the caller as soon as it's available.
                yield return idea;
            }
        }

        // Retrieves a single investment idea by its ID.
        public async ValueTask<InvestmentIdea?> GetInvestmentIdeaAsync(int id, CancellationToken cancellationToken = default)
        {
            // SQL query to select a specific investment idea, using a parameter to prevent SQL injection.
            string sql = @"
            SELECT 
                i.Id AS InvestmentId, 
                i.Name AS InvestmentName, 
                i.Ticker, 
                i.Description AS InvestmentDescription, 
                i.CreatedAt AS InvestmentCreatedAt, 
                t.Id AS ThemeId,
                t.Name AS ThemeName, 
                t.Description AS ThemeDescription, 
                t.CreatedDate AS ThemeCreatedDate
            FROM investmentideas i
            JOIN investmentthemes t ON i.InvestmentThemeId = t.Id
            WHERE i.Id = @id";

            // Open a new connection.
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Create the command and add the id parameter.
            await using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            // Execute the command and obtain a reader.
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // If a row is found, map it to an InvestmentIdea and return it.
            if (await reader.ReadAsync(cancellationToken))
            {
                return new InvestmentIdea
                {
                    Id = reader.GetInt32("InvestmentId"),
                    Name = reader.GetString("InvestmentName"),
                    Description = reader.GetString("InvestmentDescription"),
                    Ticker = reader.GetString("Ticker"),
                    CreatedAt = reader.GetDateTime("InvestmentCreatedAt"),
                    InvestmentTheme = new InvestmentTheme
                    {
                        Id = reader.GetInt32("ThemeId"),
                        Name = reader.GetString("ThemeName"),
                        Description = reader.GetString("ThemeDescription"),
                        CreatedDate = reader.GetDateTime("ThemeCreatedDate")
                    }
                };
            }

            // Return null if no matching record is found.
            return null;
        }
    }
}