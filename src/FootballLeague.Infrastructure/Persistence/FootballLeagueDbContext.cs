using FootballLeague.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Persistence;

public class FootballLeagueDbContext : DbContext
{
    public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FootballLeagueDbContext).Assembly);
    }
}