using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.Places;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Authorize]
public class PlacesController : ControllerBase
{
    private readonly PlaceService _placeService;

    public PlacesController(PlaceService placeService)
    {
        _placeService = placeService;
    }

    [HttpGet("api/journeys/{journeyId:guid}/places")]
    public async Task<IActionResult> GetPlaces(Guid journeyId)
    {
        var coupleId = User.GetCoupleId();
        var result = await _placeService.GetByJourneyAsync(coupleId, journeyId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost("api/journeys/{journeyId:guid}/places")]
    public async Task<IActionResult> Create(Guid journeyId, [FromBody] CreatePlaceRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _placeService.CreateAsync(coupleId, journeyId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Created($"api/places/{result.Data!.Id}", result.Data);
    }

    [HttpPut("api/places/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlaceRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _placeService.UpdateAsync(coupleId, id, request);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("api/places/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _placeService.DeleteAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }

    [HttpGet("api/places/map")]
    public async Task<IActionResult> GetMapPlaces()
    {
        var coupleId = User.GetCoupleId();
        var result = await _placeService.GetMapPlacesAsync(coupleId);
        return Ok(result);
    }
}
