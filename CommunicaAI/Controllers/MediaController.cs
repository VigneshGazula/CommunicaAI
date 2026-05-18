using CommunicaAI.Data;
using CommunicaAI.DTO.Media;
using CommunicaAI.Models;
using CommunicaAI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public MediaController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("onboarding")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(200_000_000)]
        public async Task<ActionResult<MediaOnboardingResponse>> UploadOnboardingMedia(
        [FromForm] MediaOnboardingUploadRequest request)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Invalid token." });

            if (request.AudioFile == null || request.AudioFile.Length == 0)
                return BadRequest(new { message = "Audio file is required." });

            if (request.VideoFile == null || request.VideoFile.Length == 0)
                return BadRequest(new { message = "Video file is required." });

            var profile = await _context.UserMediaProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                profile = new UserMediaProfile
                {
                    UserId = userId
                };
                _context.UserMediaProfiles.Add(profile);
            }

            if (!string.IsNullOrWhiteSpace(profile.AudioPublicId))
            {
                await _cloudinaryService.DeleteAsync(profile.AudioPublicId, CloudinaryDotNet.Actions.ResourceType.Video);
            }

            if (!string.IsNullOrWhiteSpace(profile.VideoPublicId))
            {
                await _cloudinaryService.DeleteAsync(profile.VideoPublicId, CloudinaryDotNet.Actions.ResourceType.Video);
            }

            var audioUpload = await _cloudinaryService.UploadAudioAsync(request.AudioFile, userId);
            var videoUpload = await _cloudinaryService.UploadVideoAsync(request.VideoFile, userId);

            profile.AudioUrl = audioUpload.Url;
            profile.AudioPublicId = audioUpload.PublicId;
            profile.AudioContentType = request.AudioFile.ContentType;
            profile.AudioSizeBytes = request.AudioFile.Length;
            profile.AudioUploadedAtUtc = DateTime.UtcNow;

            profile.VideoUrl = videoUpload.Url;
            profile.VideoPublicId = videoUpload.PublicId;
            profile.VideoContentType = request.VideoFile.ContentType;
            profile.VideoSizeBytes = request.VideoFile.Length;
            profile.VideoUploadedAtUtc = DateTime.UtcNow;

            profile.UpdatedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new MediaOnboardingResponse
            {
                UserId = userId,
                AudioUrl = profile.AudioUrl,
                AudioPublicId = profile.AudioPublicId,
                VideoUrl = profile.VideoUrl,
                VideoPublicId = profile.VideoPublicId,
                IsCompleted = profile.IsCompleted
            });
        }

        [HttpGet("me")]
        public async Task<ActionResult<MediaOnboardingResponse>> GetMyMediaProfile()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Invalid token." });

            var profile = await _context.UserMediaProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
                return NotFound(new { message = "Media profile not found." });

            return Ok(new MediaOnboardingResponse
            {
                UserId = userId,
                AudioUrl = profile.AudioUrl,
                AudioPublicId = profile.AudioPublicId,
                VideoUrl = profile.VideoUrl,
                VideoPublicId = profile.VideoPublicId,
                IsCompleted = profile.IsCompleted
            });
        }
    }
}
