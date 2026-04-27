using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Rankings;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Application.Services;

public class StandardFootballRankingCalculator : IRankingCalculator
{
    public IReadOnlyCollection<TeamRankingDto> Calculate(IEnumerable<Team> teams, IEnumerable<Match> matches)
    {
        ArgumentNullException.ThrowIfNull(teams);
        ArgumentNullException.ThrowIfNull(matches);

        var teamRankingsByTeamId = CreateTeamRankings(teams);

        foreach (var match in matches)
        {
            ApplyMatchResult(match, teamRankingsByTeamId);
        }

        var orderedRankings = teamRankingsByTeamId.Values
            .OrderByDescending(teamRanking => teamRanking.Points)
            .ThenByDescending(teamRanking => teamRanking.GoalDifference)
            .ThenByDescending(teamRanking => teamRanking.GoalsFor)
            .ThenBy(teamRanking => teamRanking.TeamName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        AssignPositions(orderedRankings);

        return orderedRankings;
    }

    private static Dictionary<int, TeamRankingDto> CreateTeamRankings(IEnumerable<Team> teams)
    {
        return teams.ToDictionary(
            team => team.Id,
            team => new TeamRankingDto
            {
                TeamId = team.Id,
                TeamName = team.Name
            });
    }

    private static void ApplyMatchResult(Match match, IReadOnlyDictionary<int, TeamRankingDto> teamRankingsByTeamId)
    {
        var homeTeamRanking = GetTeamRankingOrThrow(teamRankingsByTeamId, match.HomeTeamId, "Home");
        var awayTeamRanking = GetTeamRankingOrThrow(teamRankingsByTeamId, match.AwayTeamId, "Away");

        homeTeamRanking.PlayedMatches++;
        awayTeamRanking.PlayedMatches++;

        homeTeamRanking.GoalsFor += match.HomeScore;
        homeTeamRanking.GoalsAgainst += match.AwayScore;
        homeTeamRanking.GoalDifference = homeTeamRanking.GoalsFor - homeTeamRanking.GoalsAgainst;

        awayTeamRanking.GoalsFor += match.AwayScore;
        awayTeamRanking.GoalsAgainst += match.HomeScore;
        awayTeamRanking.GoalDifference = awayTeamRanking.GoalsFor - awayTeamRanking.GoalsAgainst;

        if (match.HomeScore > match.AwayScore)
        {
            homeTeamRanking.Wins++;
            homeTeamRanking.Points += 3;
            awayTeamRanking.Losses++;
            return;
        }

        if (match.HomeScore < match.AwayScore)
        {
            awayTeamRanking.Wins++;
            awayTeamRanking.Points += 3;
            homeTeamRanking.Losses++;
            return;
        }

        homeTeamRanking.Draws++;
        awayTeamRanking.Draws++;
        homeTeamRanking.Points++;
        awayTeamRanking.Points++;
    }

    private static TeamRankingDto GetTeamRankingOrThrow(
        IReadOnlyDictionary<int, TeamRankingDto> teamRankingsByTeamId,
        int teamId,
        string teamRole)
    {
        if (!teamRankingsByTeamId.TryGetValue(teamId, out var ranking))
        {
            throw new InvalidOperationException($"{teamRole} team '{teamId}' was not found in the provided team list.");
        }

        return ranking;
    }

    private static void AssignPositions(IList<TeamRankingDto> orderedRankings)
    {
        for (var index = 0; index < orderedRankings.Count; index++)
        {
            orderedRankings[index].Position = index + 1;
        }
    }
}