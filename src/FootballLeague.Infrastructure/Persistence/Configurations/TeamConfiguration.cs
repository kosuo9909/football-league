using FootballLeague.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(team => team.Id);

        builder.Property(team => team.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(team => team.Name)
            .IsUnique();
    }
}