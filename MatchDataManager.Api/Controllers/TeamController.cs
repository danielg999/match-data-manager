using MatchDataManager.Api.Entities;
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
    public IActionResult Create([FromBody] TeamCreateDto team)
    {
        var id = _teamService.Create(team);

        return Created($"api/team/{id}", null);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _teamService.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var teamsDtos = _teamService.GetAll();

        return Ok(teamsDtos);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] Guid id)
    {
        var teamDto = _teamService.Get(id);

        if (teamDto is null)
        {
            return NotFound();
        }

        return Ok(teamDto);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] TeamUpdateDto teamDto)
    {
        _teamService.Update(id, teamDto);
        return Ok(teamDto);
    }
}