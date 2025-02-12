using System.Runtime.CompilerServices;
using ScratchPad.Models;
using LanguageExt;

public interface IInvestmentIdeaRepository
{
    IAsyncEnumerable<InvestmentIdea> GetInvestmentIdeas(CancellationToken cancellationToken = default);
    ValueTask<Option<InvestmentIdea>> GetInvestmentIdeaById(int id, CancellationToken cancellationToken = default);

}
