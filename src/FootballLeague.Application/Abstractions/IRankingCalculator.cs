using FootballLeague.Application.Contracts.Rankings;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Abstractions;

public interface IRankingCalculator
{
    IReadOnlyCollection<TeamRankingDto> Calculate(IEnumerable<Team> teams, IEnumerable<Match> matches);
}