using FootballLeague.Application.Services;
using FootballLeague.Domain.Entities;

namespace FootballLeague.Tests;

public class StandardFootballRankingCalculatorTests
{
    private readonly StandardFootballRankingCalculator _calculator = new();

    // --- Arrange helpers ---

    private static Team MakeTeam(int id, string name) => new() { Id = id, Name = name };

    private static Match MakeMatch(int homeTeamId, int awayTeamId, int homeScore, int awayScore) => new()
    {
        HomeTeamId = homeTeamId,
        AwayTeamId = awayTeamId,
        HomeScore = homeScore,
        AwayScore = awayScore
    };

    // --- Tests ---

    [Fact]
    public void Calculate_WithNoMatches_ReturnsAllTeamsWithZeroPoints()
    {
        // Arrange
        var teams = new[] { MakeTeam(1, "CSKA"), MakeTeam(2, "Levski") };
        var matches = Array.Empty<Match>();

        // Act
        var rankings = _calculator.Calculate(teams, matches);

        // Assert
        Assert.Equal(2, rankings.Count);
        Assert.All(rankings, r => Assert.Equal(0, r.Points));
    }

    [Fact]
    public void Calculate_HomeTeamWins_HomeTeamGetsThreePoints()
    {
        // Arrange
        var teams = new[] { MakeTeam(1, "CSKA"), MakeTeam(2, "Levski") };
        var matches = new[] { MakeMatch(homeTeamId: 1, awayTeamId: 2, homeScore: 2, awayScore: 0) };

        // Act
        var rankings = _calculator.Calculate(teams, matches);

        // Assert
        var cska = rankings.Single(r => r.TeamId == 1);
        var levski = rankings.Single(r => r.TeamId == 2);

        Assert.Equal(3, cska.Points);
        Assert.Equal(0, levski.Points);
    }

    [Fact]
    public void Calculate_Draw_BothTeamsGetOnePoint()
    {
        // Arrange
        var teams = new[] { MakeTeam(1, "CSKA"), MakeTeam(2, "Levski") };
        var matches = new[] { MakeMatch(homeTeamId: 1, awayTeamId: 2, homeScore: 1, awayScore: 1) };

        // Act
        var rankings = _calculator.Calculate(teams, matches);

        // Assert
        Assert.All(rankings, r => Assert.Equal(1, r.Points));
    }

    [Fact]
    public void Calculate_TeamsRankedByPointsDescending()
    {
        // Arrange: CSKA wins, Levski loses, so CSKA should be first
        var teams = new[] { MakeTeam(1, "CSKA"), MakeTeam(2, "Levski") };
        var matches = new[] { MakeMatch(homeTeamId: 1, awayTeamId: 2, homeScore: 3, awayScore: 1) };

        // Act
        var rankings = _calculator.Calculate(teams, matches).ToList();

        // Assert
        Assert.Equal(1, rankings[0].TeamId); // CSKA first
        Assert.Equal(2, rankings[1].TeamId); // Levski second
    }

    [Fact]
    public void Calculate_GoalDifferenceIsCorrect()
    {
        // Arrange
        var teams = new[] { MakeTeam(1, "CSKA"), MakeTeam(2, "Levski") };
        var matches = new[] { MakeMatch(homeTeamId: 1, awayTeamId: 2, homeScore: 3, awayScore: 1) };

        // Act
        var rankings = _calculator.Calculate(teams, matches);

        // Assert
        var cska = rankings.Single(r => r.TeamId == 1);
        var levski = rankings.Single(r => r.TeamId == 2);

        Assert.Equal(2, cska.GoalDifference);   // scored 3, conceded 1
        Assert.Equal(-2, levski.GoalDifference); // scored 1, conceded 3
    }

    [Fact]
    public void Calculate_MatchWithTeamNotInList_ThrowsInvalidOperationException()
    {
        // Arrange: match references team id 99 which does not exist
        var teams = new[] { MakeTeam(1, "CSKA") };
        var matches = new[] { MakeMatch(homeTeamId: 1, awayTeamId: 99, homeScore: 1, awayScore: 0) };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _calculator.Calculate(teams, matches));
    }
}
