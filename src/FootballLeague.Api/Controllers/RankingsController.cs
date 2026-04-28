using FootballLeague.Api.Models;
using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Rankings;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RankingsController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public RankingsController(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    /// <summary>
    /// Gets the current league standings, ordered by points, goal difference, and goals scored.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A ranked list of all teams.</returns>
    /// <response code="200">Returns the current league standings.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRankings(CancellationToken cancellationToken)
    {
        var rankings = await _rankingService.GetRankingsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<TeamRankingDto>>.Succeeded(rankings));
    }
}
