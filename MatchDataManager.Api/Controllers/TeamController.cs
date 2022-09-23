using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchDataManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost]
    public IActionResult AddTeam(Team team)
    {
        _teamService.AddTeam(team);
        return CreatedAtAction(nameof(GetById), new {id = team.Id}, team);
    }

    [HttpDelete]
    public IActionResult DeleteTeam(Guid teamId)
    {
        _teamService.DeleteTeam(teamId);
        return NoContent();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _teamService.GetAllTeams();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var location = _teamService.GetTeamById(id);
        if (location is null)
        {
            return NotFound();
        }

        return Ok(location);
    }

    [HttpPut]
    public IActionResult UpdateTeam(Team team)
    {
        _teamService.UpdateTeam(team);
        return Ok(team);
    }
}