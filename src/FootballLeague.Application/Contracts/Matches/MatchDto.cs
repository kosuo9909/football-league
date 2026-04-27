namespace FootballLeague.Application.Contracts.Matches;

public class MatchDto
{
    public int Id { get; set; }

    public int HomeTeamId { get; set; }

    public string HomeTeamName { get; set; } = string.Empty;

    public int AwayTeamId { get; set; }

    public string AwayTeamName { get; set; } = string.Empty;

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public DateTime PlayedOnUtc { get; set; }
}