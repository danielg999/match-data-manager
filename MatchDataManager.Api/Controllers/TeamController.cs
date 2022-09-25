using MatchDataManager.Api.Models;
using MatchDataManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchDataManager.Api.Controllers;

[ApiController]
[Route("api/team")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeamCreateDto team)
    {
        var id = await _teamService.Create(team);

        return Created($"api/team/{id}", null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _teamService.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teamsDtos = await _teamService.GetAll();

        return Ok(teamsDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var teamDto = await _teamService.Get(id);

        if (teamDto is null)
        {
            return NotFound();
        }

        return Ok(teamDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TeamUpdateDto teamDto)
    {
        await _teamService.Update(teamDto);
        return Ok(teamDto);
    }
}