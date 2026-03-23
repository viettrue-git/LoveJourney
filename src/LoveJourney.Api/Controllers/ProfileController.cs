using LoveJourney.Api.Extensions;
using LoveJourney.Application.Common.Interfaces;
using LoveJourney.Application.DTOs.Profile;
using LoveJourney.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoveJourney.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;
    private readonly IFileStorageService _fileStorage;

    public ProfileController(ProfileService profileService, IFileStorageService fileStorage)
    {
        _profileService = profileService;
        _fileStorage = fileStorage;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var coupleId = User.GetCoupleId();
        var result = await _profileService.GetProfileAsync(coupleId);
        if (!result.Success) return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var coupleId = User.GetCoupleId();
        var result = await _profileService.UpdateProfileAsync(coupleId, request);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost("photo")]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        if (file.Length == 0) return BadRequest(new { error = "File trống." });
        if (file.Length > 5 * 1024 * 1024) return BadRequest(new { error = "File quá lớn (tối đa 5MB)." });

        var coupleId = User.GetCoupleId();
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"profile_{coupleId}{ext}";

        using var stream = file.OpenReadStream();
        var path = await _fileStorage.SaveAsync(stream, fileName, "profiles");
        var url = _fileStorage.GetFullUrl(path);

        var result = await _profileService.UpdatePhotoUrlAsync(coupleId, url);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Ok(result.Data);
    }
}
