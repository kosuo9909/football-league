namespace FootballLeague.Application.Contracts.Matches;

public class UpdateMatchRequest
{
    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public DateTime PlayedOnUtc { get; set; }
}