using MatchDataManager.Api.Models;
using MatchDataManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchDataManager.Api.Controllers;

[ApiController]
[Route("api/location")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] LocationCreateDto locationDto)
    {
        var id = _locationService.Create(locationDto);

        return Created($"api/location/{id}", null);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _locationService.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var locationsDtos = _locationService.GetAll();

        return Ok(locationsDtos);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] Guid id)
    {
        var locationDto = _locationService.Get(id);

        if (locationDto is null)
        {
            return NotFound();
        }

        return Ok(locationDto);
    }

    [HttpPut]
    public IActionResult Update([FromBody] LocationUpdateDto locationDto)
    {
        _locationService.Update(locationDto);

        return Ok(locationDto);
    }
}