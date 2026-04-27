using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Teams;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Services;

public class TeamService : ITeamService
{
    private readonly FootballLeagueDbContext _dbContext;

    public TeamService(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TeamDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var teams = await _dbContext.Teams
            .AsNoTracking()
            .OrderBy(team => team.Name)
            .ToListAsync(cancellationToken);

        return teams.Select(ToDto).ToList();
    }

    public async Task<TeamDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var team = await _dbContext.Teams.FindAsync(new object[] { id }, cancellationToken);

        return team is null ? null : ToDto(team);
    }

    public async Task<TeamDto> CreateAsync(CreateTeamRequest request, CancellationToken cancellationToken = default)
    {
        var team = new Team { Name = request.Name };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(team);
    }

    public async Task<TeamDto?> UpdateAsync(int id, UpdateTeamRequest request, CancellationToken cancellationToken = default)
    {
        var team = await _dbContext.Teams.FindAsync(new object[] { id }, cancellationToken);

        if (team is null)
        {
            return null;
        }

        team.Name = request.Name;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToDto(team);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var team = await _dbContext.Teams.FindAsync(new object[] { id }, cancellationToken);

        if (team is null)
        {
            return false;
        }

        _dbContext.Teams.Remove(team);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static TeamDto ToDto(Team team) => new() { Id = team.Id, Name = team.Name };
}
