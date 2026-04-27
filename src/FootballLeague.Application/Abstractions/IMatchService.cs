using FootballLeague.Application.Contracts.Matches;

namespace FootballLeague.Application.Abstractions;

public interface IMatchService
{
    Task<IReadOnlyCollection<MatchDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MatchDto>> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default);

    Task<MatchDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<MatchDto> CreateAsync(CreateMatchRequest request, CancellationToken cancellationToken = default);

    Task<MatchDto?> UpdateAsync(int id, UpdateMatchRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}