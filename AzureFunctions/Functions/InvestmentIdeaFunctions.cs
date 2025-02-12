using System.Collections.Generic;               // For List<T> and IAsyncEnumerable<T>
using System.Threading.Tasks;                   // For async Task methods
using Microsoft.Azure.Functions.Worker;         // For Function and FunctionContext
using Microsoft.Azure.Functions.Worker.Http;    // For HttpRequestData and HttpResponseData
using Microsoft.Extensions.Logging;             // For logging support
using ScratchPad.Models;                        // Contains InvestmentIdea and InvestmentTheme models
using ScratchPad.AzureFunctions.Repositories;                                // Contains IInvestmentIdeaRepository and its implementation
using System.Net;                               // For HttpStatusCode

namespace ScratchPad.AzureFunctions.Functions
{
    public class InvestmentIdeaFunction
    {
        // Dependency-inject the repository and logger.
        private readonly IInvestmentIdeaRepository _repository;
        private readonly ILogger<InvestmentIdeaFunction> _logger;

        // The constructor receives the repository and logger from DI.
        public InvestmentIdeaFunction(IInvestmentIdeaRepository repository, ILogger<InvestmentIdeaFunction> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Function("GetInvestmentIdeas")]
        public async Task<HttpResponseData> GetInvestmentIdeas(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "investment-ideas")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Fetching all investment ideas with their themes...");

            // Create a list to collect investment ideas streamed from the repository.
            var ideas = new List<InvestmentIdea>();

            // Asynchronously stream each InvestmentIdea using IAsyncEnumerable.
            await foreach (var idea in _repository.GetInvestmentIdeasAsync())
            {
                ideas.Add(idea);
            }

            // Create the HTTP response with a status code of 200 (OK).
            var response = req.CreateResponse(HttpStatusCode.OK);

            // Write the list of ideas as JSON in the response.
            await response.WriteAsJsonAsync(ideas);

            return response;
        }
    }
}