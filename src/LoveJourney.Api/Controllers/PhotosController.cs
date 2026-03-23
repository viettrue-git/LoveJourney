using LoveJourney.Api.Extensions;
using LoveJourney.Application.DTOs.Photos;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/photos")]
[Authorize]
public class PhotosController : ControllerBase
{
    private readonly PhotoService _photoService;

    public PhotosController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhotos(
        [FromQuery] Guid? journeyId, [FromQuery] Guid? placeId,
        [FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var coupleId = User.GetCoupleId();
        var result = await _photoService.GetPhotosAsync(coupleId, journeyId, placeId, page, size);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(
        [FromForm] List<IFormFile> files,
        [FromForm] Guid? journeyId,
        [FromForm] Guid? placeId)
    {
        var coupleId = User.GetCoupleId();
        var results = new List<PhotoResponse>();

        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            if (file.Length > 10 * 1024 * 1024)
                return BadRequest(new { error = $"File {file.FileName} quá lớn (tối đa 10MB)." });

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { error = $"File {file.FileName} không phải ảnh hợp lệ." });

            using var stream = file.OpenReadStream();
            var result = await _photoService.UploadAsync(
                coupleId, journeyId, placeId,
                stream, file.FileName, file.ContentType, file.Length);

            if (result.Success && result.Data != null)
                results.Add(result.Data);
        }

        return Ok(results);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCaption(Guid id, [FromBody] UpdatePhotoCaptionRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _photoService.UpdateCaptionAsync(coupleId, id, request.Caption);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var coupleId = User.GetCoupleId();
        var result = await _photoService.DeleteAsync(coupleId, id);
        if (!result.Success) return NotFound(new { error = result.Error });
        return NoContent();
    }
}
