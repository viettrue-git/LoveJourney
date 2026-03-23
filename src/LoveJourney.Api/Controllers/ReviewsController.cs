using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.Reviews;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/places/{placeId:guid}/review")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid placeId)
    {
        var coupleId = User.GetCoupleId();
        var result = await _reviewService.GetByPlaceAsync(coupleId, placeId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(Guid placeId, [FromBody] CreateReviewRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _reviewService.CreateOrUpdateAsync(coupleId, placeId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid placeId)
    {
        var coupleId = User.GetCoupleId();
        var result = await _reviewService.DeleteAsync(coupleId, placeId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
