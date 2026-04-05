using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.BlogPosts;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogPostsController : ControllerBase
{
    private readonly BlogPostService _blogPostService;

    public BlogPostsController(BlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    // Public endpoints (no auth)
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublished([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var result = await _blogPostService.GetPublishedAsync(page, size);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublishedById(Guid id)
    {
        var result = await _blogPostService.GetPublishedByIdAsync(id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    // Auth endpoints (couple manages own posts)
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyPosts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var coupleId = User.GetCoupleId();
        var result = await _blogPostService.GetMyPostsAsync(coupleId, page, size);
        return Ok(result);
    }

    [HttpGet("my/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMyPostById(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _blogPostService.GetMyPostByIdAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateBlogPostRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _blogPostService.CreateAsync(coupleId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GetPublishedById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlogPostRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _blogPostService.UpdateAsync(coupleId, id, request);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _blogPostService.DeleteAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
