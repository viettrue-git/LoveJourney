using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.Journeys;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/journeys")]
[Authorize]
public class JourneysController : ControllerBase
{
    private readonly JourneyService _journeyService;

    public JourneysController(JourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetJourneys(
        [FromQuery] string? type, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to,
        [FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.GetJourneysAsync(coupleId, type, from, to, page, size);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJourney(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.GetJourneyByIdAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpGet("timeline")]
    public async Task<IActionResult> GetTimeline()
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.GetTimelineAsync(coupleId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJourneyRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.CreateAsync(coupleId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GetJourney), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJourneyRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.UpdateAsync(coupleId, id, request);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _journeyService.DeleteAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
