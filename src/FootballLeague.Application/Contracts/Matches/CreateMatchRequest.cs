using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Application.Contracts.Matches;

public class CreateMatchRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "HomeTeamId must be a positive integer.")]
    public int HomeTeamId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "AwayTeamId must be a positive integer.")]
    public int AwayTeamId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "HomeScore must be zero or greater.")]
    public int HomeScore { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "AwayScore must be zero or greater.")]
    public int AwayScore { get; set; }

    [Required]
    public DateTime PlayedOnUtc { get; set; }
}