using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Rankings;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Services;

public class RankingService : IRankingService
{
    private readonly FootballLeagueDbContext _dbContext;
    private readonly IRankingCalculator _rankingCalculator;

    public RankingService(FootballLeagueDbContext dbContext, IRankingCalculator rankingCalculator)
    {
        _dbContext = dbContext;
        _rankingCalculator = rankingCalculator;
    }

    public async Task<IReadOnlyCollection<TeamRankingDto>> GetRankingsAsync(CancellationToken cancellationToken = default)
    {
        var teams = await _dbContext.Teams
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var matches = await _dbContext.Matches
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _rankingCalculator.Calculate(teams, matches);
    }
}
