# Football League API

A REST API for managing football league teams, match results, and standings.

Built with **.NET 6**, **Entity Framework Core 6**, and **SQL Server LocalDB**.

---

## Design Notes

The ranking calculation uses the **Strategy pattern** — `IRankingCalculator` defines the contract, and `StandardFootballRankingCalculator` is the implementation injected at runtime via the DI container. A different points system (e.g. 2 points for a win) could be swapped in by providing a new implementation without touching the rest of the codebase.

The solution is split into four layers — Domain, Application, Infrastructure, and Api — so that business logic has no dependency on the database or HTTP stack.

---

## Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6)
- SQL Server LocalDB (included with Visual Studio, or install via [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads))

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/kosuo9909/football-league.git
cd football-league
```

### 2. Apply database migrations

```bash
dotnet tool restore
dotnet ef database update --project src/FootballLeague.Infrastructure --startup-project src/FootballLeague.Api
```

This creates the `FootballLeagueDb` database on your local SQL Server instance.

### 3. Run the API

```bash
cd src/FootballLeague.Api
dotnet run
```

### 4. Open Swagger UI

Navigate to `https://localhost:7250/swagger`

---

## Sample Data

When running in Development mode, the API automatically seeds the database with four football teams and six matches on first startup — no manual steps required.

To reseed (e.g. after deleting data), drop and recreate the database:

```bash
dotnet ef database drop --project src/FootballLeague.Infrastructure --startup-project src/FootballLeague.Api --force
dotnet ef database update --project src/FootballLeague.Infrastructure --startup-project src/FootballLeague.Api
```

Then restart the API.

---

## Running Tests

```bash
dotnet test
```

---

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/teams` | Get all teams |
| GET | `/api/teams/{id}` | Get team by id |
| POST | `/api/teams` | Create a team |
| PUT | `/api/teams/{id}` | Update a team |
| DELETE | `/api/teams/{id}` | Delete a team |
| GET | `/api/matches` | Get all matches |
| GET | `/api/matches/{id}` | Get match by id |
| GET | `/api/matches/team/{teamId}` | Get matches for a team |
| POST | `/api/matches` | Record a match result |
| PUT | `/api/matches/{id}` | Update a match result |
| DELETE | `/api/matches/{id}` | Delete a match |
| GET | `/api/rankings` | Get current league standings |

---

## Project Structure

```
src/
  FootballLeague.Domain          # Entities (Team, Match)
  FootballLeague.Application     # Interfaces, DTOs, ranking calculator
  FootballLeague.Infrastructure  # EF Core, services, database
  FootballLeague.Api             # Controllers, middleware, Program.cs
tests/
  FootballLeague.Tests           # Unit tests for ranking calculator
```
