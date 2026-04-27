using FootballLeague.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(match => match.Id);

        builder.Property(match => match.PlayedOnUtc)
            .IsRequired();

        builder.Property(match => match.HomeScore)
            .IsRequired();

        builder.Property(match => match.AwayScore)
            .IsRequired();

        builder.HasOne(match => match.HomeTeam)
            .WithMany()
            .HasForeignKey(match => match.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(match => match.AwayTeam)
            .WithMany()
            .HasForeignKey(match => match.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasCheckConstraint("CK_Matches_DifferentTeams", "[HomeTeamId] <> [AwayTeamId]");
        builder.HasCheckConstraint("CK_Matches_NonNegativeScores", "[HomeScore] >= 0 AND [AwayScore] >= 0");

        builder.HasIndex(match => match.PlayedOnUtc);
    }
}