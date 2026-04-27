using FootballLeague.Application.Contracts.Teams;

namespace FootballLeague.Application.Abstractions;

public interface ITeamService
{
    Task<IReadOnlyCollection<TeamDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TeamDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TeamDto> CreateAsync(CreateTeamRequest request, CancellationToken cancellationToken = default);

    Task<TeamDto?> UpdateAsync(int id, UpdateTeamRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}