using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.Anniversaries;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/anniversaries")]
[Authorize]
public class AnniversariesController : ControllerBase
{
    private readonly AnniversaryService _anniversaryService;

    public AnniversariesController(AnniversaryService anniversaryService)
    {
        _anniversaryService = anniversaryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coupleId = User.GetCoupleId();
        var result = await _anniversaryService.GetAllAsync(coupleId);
        return Ok(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int days = 30)
    {
        var coupleId = User.GetCoupleId();
        var result = await _anniversaryService.GetUpcomingAsync(coupleId, days);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnniversaryRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _anniversaryService.CreateAsync(coupleId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Created($"api/anniversaries/{result.Data!.Id}", result.Data);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnniversaryRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _anniversaryService.UpdateAsync(coupleId, id, request);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _anniversaryService.DeleteAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
