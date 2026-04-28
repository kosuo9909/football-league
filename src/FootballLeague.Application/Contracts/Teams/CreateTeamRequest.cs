using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Application.Contracts.Teams;

public class CreateTeamRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}