using FootballLeague.Application.Contracts.Rankings;

namespace FootballLeague.Application.Abstractions;

public interface IRankingService
{
    Task<IReadOnlyCollection<RankingRowDto>> GetRankingsAsync(CancellationToken cancellationToken = default);
}