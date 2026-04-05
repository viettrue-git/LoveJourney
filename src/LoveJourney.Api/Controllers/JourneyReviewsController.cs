using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.JourneyReviews;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/journeys/{journeyId:guid}/review")]
[Authorize]
public class JourneyReviewsController : ControllerBase
{
    private readonly JourneyReviewService _service;

    public JourneyReviewsController(JourneyReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid journeyId)
    {
        var coupleId = User.GetCoupleId();
        var result = await _service.GetByJourneyAsync(coupleId, journeyId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(Guid journeyId, [FromBody] CreateJourneyReviewRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _service.CreateOrUpdateAsync(coupleId, journeyId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid journeyId)
    {
        var coupleId = User.GetCoupleId();
        var result = await _service.DeleteAsync(coupleId, journeyId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
