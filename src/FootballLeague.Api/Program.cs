using FootballLeague.Api.Infrastructure;
using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Services;
using FootballLeague.Infrastructure.Persistence;
using FootballLeague.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<FootballLeagueDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IRankingCalculator, StandardFootballRankingCalculator>();

var app = builder.Build();

app.UseMiddleware<FootballLeague.Api.Infrastructure.GlobalExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FootballLeagueDbContext>();
    DatabaseSeeder.SeedIfEmpty(dbContext);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
