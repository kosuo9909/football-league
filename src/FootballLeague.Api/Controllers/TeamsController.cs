using FootballLeague.Api.Models;
using FootballLeague.Application.Abstractions;
using FootballLeague.Application.Contracts.Teams;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>
    /// Gets all teams ordered by name.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all teams.</returns>
    /// <response code="200">Returns the list of teams.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var teams = await _teamService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<TeamDto>>.Succeeded(teams));
    }

    /// <summary>
    /// Gets a team by identifier.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The team if found.</returns>
    /// <response code="200">Returns the team.</response>
    /// <response code="404">If the team does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var team = await _teamService.GetByIdAsync(id, cancellationToken);

        if (team is null)
        {
            return NotFound(ApiResponse<TeamDto>.Failed($"Team with id {id} was not found."));
        }

        return Ok(ApiResponse<TeamDto>.Succeeded(team));
    }

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="request">The team to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created team.</returns>
    /// <response code="201">Returns the newly created team.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = team.Id }, ApiResponse<TeamDto>.Succeeded(team));
    }

    /// <summary>
    /// Updates an existing team.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="request">The updated team data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated team if found.</returns>
    /// <response code="200">Returns the updated team.</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the team does not exist.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamService.UpdateAsync(id, request, cancellationToken);

        if (team is null)
        {
            return NotFound(ApiResponse<TeamDto>.Failed($"Team with id {id} was not found."));
        }

        return Ok(ApiResponse<TeamDto>.Succeeded(team));
    }

    /// <summary>
    /// Deletes a team by identifier.
    /// </summary>
    /// <param name="id">The team identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if deleted, not found otherwise.</returns>
    /// <response code="204">Team successfully deleted.</response>
    /// <response code="404">If the team does not exist.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _teamService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound(ApiResponse<TeamDto>.Failed($"Team with id {id} was not found."));
        }

        return NoContent();
    }
}
