using FootballLeague.Application.Contracts.Rankings;

namespace FootballLeague.Application.Abstractions;

public interface IRankingService
{
    Task<IReadOnlyCollection<TeamRankingDto>> GetRankingsAsync(CancellationToken cancellationToken = default);
}