using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Application.Contracts.Matches;

public class CreateMatchRequest : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "HomeTeamId must be a positive integer.")]
    public int? HomeTeamId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "AwayTeamId must be a positive integer.")]
    public int? AwayTeamId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "HomeScore must be zero or greater.")]
    public int? HomeScore { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "AwayScore must be zero or greater.")]
    public int? AwayScore { get; set; }

    [Required]
    public DateTime? PlayedOnUtc { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HomeTeamId.HasValue && AwayTeamId.HasValue && HomeTeamId.Value == AwayTeamId.Value)
        {
            yield return new ValidationResult(
                "HomeTeamId and AwayTeamId must be different.",
                new[] { nameof(HomeTeamId), nameof(AwayTeamId) });
        }
    }
}