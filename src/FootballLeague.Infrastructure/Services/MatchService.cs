using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Matches;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Services;

public class MatchService : IMatchService
{
    private readonly FootballLeagueDbContext _dbContext;

    public MatchService(FootballLeagueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<MatchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var matches = await _dbContext.Matches
            .AsNoTracking()
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .OrderByDescending(match => match.PlayedOnUtc)
            .ToListAsync(cancellationToken);

        return matches.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyCollection<MatchDto>> GetByTeamIdAsync(int teamId, CancellationToken cancellationToken = default)
    {
        var matches = await _dbContext.Matches
            .AsNoTracking()
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .Where(match => match.HomeTeamId == teamId || match.AwayTeamId == teamId)
            .OrderByDescending(match => match.PlayedOnUtc)
            .ToListAsync(cancellationToken);

        return matches.Select(ToDto).ToList();
    }

    public async Task<MatchDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var match = await _dbContext.Matches
            .AsNoTracking()
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .FirstOrDefaultAsync(match => match.Id == id, cancellationToken);

        return match is null ? null : ToDto(match);
    }

    public async Task<MatchDto> CreateAsync(CreateMatchRequest request, CancellationToken cancellationToken = default)
    {
        // Fetch teams upfront so we have their names for the returned DTO,
        // and so we fail early with a clear message if either team does not exist.
        var homeTeam = await _dbContext.Teams.FirstOrDefaultAsync(team => team.Id == request.HomeTeamId, cancellationToken);
        if (homeTeam is null)
        {
            throw new InvalidOperationException($"Home team with id {request.HomeTeamId} was not found.");
        }

        var awayTeam = await _dbContext.Teams.FirstOrDefaultAsync(team => team.Id == request.AwayTeamId, cancellationToken);
        if (awayTeam is null)
        {
            throw new InvalidOperationException($"Away team with id {request.AwayTeamId} was not found.");
        }

        var match = new Match
        {
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            HomeScore = request.HomeScore,
            AwayScore = request.AwayScore,
            PlayedOnUtc = request.PlayedOnUtc
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Attach the already-fetched teams so ToDto can read their names
        // without an extra round-trip to the database.
        match.HomeTeam = homeTeam;
        match.AwayTeam = awayTeam;

        return ToDto(match);
    }

    public async Task<MatchDto?> UpdateAsync(int id, UpdateMatchRequest request, CancellationToken cancellationToken = default)
    {
        var match = await _dbContext.Matches.FirstOrDefaultAsync(match => match.Id == id, cancellationToken);

        if (match is null)
        {
            return null;
        }

        match.HomeTeamId = request.HomeTeamId;
        match.AwayTeamId = request.AwayTeamId;
        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.PlayedOnUtc = request.PlayedOnUtc;

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Team IDs may have changed, so reload both navigation properties
        // from the database to get fresh team names for the returned DTO.
        await _dbContext.Entry(match).Reference(m => m.HomeTeam).LoadAsync(cancellationToken);
        await _dbContext.Entry(match).Reference(m => m.AwayTeam).LoadAsync(cancellationToken);

        return ToDto(match);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var match = await _dbContext.Matches.FirstOrDefaultAsync(match => match.Id == id, cancellationToken);

        if (match is null)
        {
            return false;
        }

        _dbContext.Matches.Remove(match);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static MatchDto ToDto(Match match) => new()
    {
        Id = match.Id,
        HomeTeamId = match.HomeTeamId,
        HomeTeamName = match.HomeTeam!.Name,
        AwayTeamId = match.AwayTeamId,
        AwayTeamName = match.AwayTeam!.Name,
        HomeScore = match.HomeScore,
        AwayScore = match.AwayScore,
        PlayedOnUtc = match.PlayedOnUtc
    };
}
