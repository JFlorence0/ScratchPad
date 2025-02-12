using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using http.context;
using MySqlConnector;
using System.Collections.Generic;
using System.Net;

namespace http.Functions
{
    public class InvestmentIdeaFunction
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvestmentIdeaFunction> _logger;

        public InvestmentIdeaFunction(ApplicationDbContext context, ILogger<InvestmentIdeaFunction> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Function("GetInvestmentIdeas")]
        public async Task<HttpResponseData> GetInvestmentIdeas(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "investment-ideas")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Fetching all investment ideas with their themes...");

            var connectionString = _context.Database.GetConnectionString();
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = """
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
                JOIN investmentthemes t ON i.InvestmentThemeId = t.Id;
            """;

            await using var command = new MySqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            var ideas = new List<object>();

            while (await reader.ReadAsync())
            {
                var investmentIdea = new
                {
                    Id = reader.GetInt32("InvestmentId"),
                    Name = reader.GetString("InvestmentName"),
                    Ticker = reader.GetString("Ticker"),
                    Description = reader.GetString("InvestmentDescription"),
                    CreatedAt = reader.GetDateTime("InvestmentCreatedAt"),
                    InvestmentTheme = new
                    {
                        Id = reader.GetInt32("ThemeId"),
                        Name = reader.GetString("ThemeName"),
                        Description = reader.GetString("ThemeDescription"),
                        CreatedDate = reader.GetDateTime("ThemeCreatedDate")
                    }
                };

                ideas.Add(investmentIdea);
            }

            // Create the HTTP response using the isolated worker pattern.
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(ideas);
            return response;
        }
    }
}