using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using http.context;

namespace http
{
    public class DbTestFunction
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public DbTestFunction(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<DbTestFunction>();
        }

        [Function("DbTestFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "dbtest")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            try
            {
                bool canConnect = await _dbContext.Database.CanConnectAsync();
                string message = canConnect
                    ? "Database connectivity test: Success"
                    : "Database connectivity test: Failed";
                // Asynchronously write the message to the response body
                await response.Body.WriteAsync(Encoding.UTF8.GetBytes(message));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error testing database connectivity.");
                await response.Body.WriteAsync(Encoding.UTF8.GetBytes("Database connectivity test: Exception occurred"));
            }
            return response;
        }
    }
}
