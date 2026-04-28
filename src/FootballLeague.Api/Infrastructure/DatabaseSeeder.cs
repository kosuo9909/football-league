using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Persistence;

namespace FootballLeague.Api.Infrastructure;

public static class DatabaseSeeder
{
    public static void SeedIfEmpty(FootballLeagueDbContext dbContext)
    {
        if (dbContext.Teams.Any())
        {
            return;
        }

        var cska = new Team { Name = "CSKA Sofia" };
        var levski = new Team { Name = "Levski Sofia" };
        var ludogorets = new Team { Name = "Ludogorets Razgrad" };
        var botev = new Team { Name = "Botev Plovdiv" };

        dbContext.Teams.AddRange(cska, levski, ludogorets, botev);
        dbContext.SaveChanges();

        dbContext.Matches.AddRange(
            new Match { HomeTeam = cska, AwayTeam = levski, HomeScore = 2, AwayScore = 1, PlayedOnUtc = new DateTime(2026, 1, 5, 15, 0, 0, DateTimeKind.Utc) },
            new Match { HomeTeam = ludogorets, AwayTeam = botev, HomeScore = 3, AwayScore = 0, PlayedOnUtc = new DateTime(2026, 1, 12, 15, 0, 0, DateTimeKind.Utc) },
            new Match { HomeTeam = levski, AwayTeam = ludogorets, HomeScore = 1, AwayScore = 1, PlayedOnUtc = new DateTime(2026, 1, 19, 15, 0, 0, DateTimeKind.Utc) },
            new Match { HomeTeam = botev, AwayTeam = cska, HomeScore = 0, AwayScore = 2, PlayedOnUtc = new DateTime(2026, 1, 26, 15, 0, 0, DateTimeKind.Utc) },
            new Match { HomeTeam = cska, AwayTeam = ludogorets, HomeScore = 2, AwayScore = 1, PlayedOnUtc = new DateTime(2026, 2, 2, 15, 0, 0, DateTimeKind.Utc) },
            new Match { HomeTeam = levski, AwayTeam = botev, HomeScore = 3, AwayScore = 1, PlayedOnUtc = new DateTime(2026, 2, 9, 15, 0, 0, DateTimeKind.Utc) }
        );

        dbContext.SaveChanges();
    }
}
