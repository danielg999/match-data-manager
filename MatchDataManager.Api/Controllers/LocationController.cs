using MatchDataManager.Api.Entities;
using MatchDataManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchDataManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    [HttpPost]
    public IActionResult AddLocation(Location location)
    {
        _locationService.AddLocation(location);
        return CreatedAtAction(nameof(GetById), new {id = location.Id}, location);
    }

    [HttpDelete]
    public IActionResult DeleteLocation(Guid locationId)
    {
        _locationService.DeleteLocation(locationId);
        return NoContent();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _locationService.GetAllLocations();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var location = _locationService.GetLocationById(id);
        if (location is null)
        {
            return NotFound();
        }

        return Ok(location);
    }

    [HttpPut]
    public IActionResult UpdateLocation(Location location)
    {
        _locationService.UpdateLocation(location);
        return Ok(location);
    }
}