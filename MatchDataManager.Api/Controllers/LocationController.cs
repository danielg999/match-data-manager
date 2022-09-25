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
    public async Task<IActionResult> Create([FromBody] LocationCreateDto locationDto)
    {
        var id = await _locationService.Create(locationDto);

        return Created($"api/location/{id}", null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _locationService.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locationsDtos = await _locationService.GetAll();

        return Ok(locationsDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var locationDto = await _locationService.Get(id);

        if (locationDto is null)
        {
            return NotFound();
        }

        return Ok(locationDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] LocationUpdateDto locationDto)
    {
        await _locationService.Update(locationDto);

        return Ok(locationDto);
    }
}