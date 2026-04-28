using FootballLeague.Api.Models;
using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Matches;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    /// <summary>
    /// Gets all matches ordered by date descending.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all matches.</returns>
    /// <response code="200">Returns the list of matches.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var matches = await _matchService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<MatchDto>>.Succeeded(matches));
    }

    /// <summary>
    /// Gets a match by identifier.
    /// </summary>
    /// <param name="id">The match identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The match if found.</returns>
    /// <response code="200">Returns the match.</response>
    /// <response code="404">If the match does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var match = await _matchService.GetByIdAsync(id, cancellationToken);

        if (match is null)
        {
            return NotFound(ApiResponse<MatchDto>.Failed($"Match with id {id} was not found."));
        }

        return Ok(ApiResponse<MatchDto>.Succeeded(match));
    }

    /// <summary>
    /// Gets all matches for a specific team.
    /// </summary>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of matches where the team played as home or away.</returns>
    /// <response code="200">Returns the list of matches for the team.</response>
    [HttpGet("team/{teamId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTeamId(int teamId, CancellationToken cancellationToken)
    {
        var matches = await _matchService.GetByTeamIdAsync(teamId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<MatchDto>>.Succeeded(matches));
    }

    /// <summary>
    /// Records a new match result.
    /// </summary>
    /// <param name="request">The match to record.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created match.</returns>
    /// <response code="201">Returns the newly recorded match.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = match.Id }, ApiResponse<MatchDto>.Succeeded(match));
    }

    /// <summary>
    /// Updates an existing match result.
    /// </summary>
    /// <param name="id">The match identifier.</param>
    /// <param name="request">The updated match data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated match if found.</returns>
    /// <response code="200">Returns the updated match.</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the match does not exist.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchService.UpdateAsync(id, request, cancellationToken);

        if (match is null)
        {
            return NotFound(ApiResponse<MatchDto>.Failed($"Match with id {id} was not found."));
        }

        return Ok(ApiResponse<MatchDto>.Succeeded(match));
    }

    /// <summary>
    /// Deletes a match by identifier.
    /// </summary>
    /// <param name="id">The match identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if deleted, not found otherwise.</returns>
    /// <response code="204">Match successfully deleted.</response>
    /// <response code="404">If the match does not exist.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _matchService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound(ApiResponse<MatchDto>.Failed($"Match with id {id} was not found."));
        }

        return NoContent();
    }
}
