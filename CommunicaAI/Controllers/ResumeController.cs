using CommunicaAI.DTO.Resume;
using CommunicaAI.Models;
using CommunicaAI.Repositories.Interfaces;
using CommunicaAI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommunicaAI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeProfileRepository _resumeRepository;
        private readonly ResumeParserService _parserService;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(
            IResumeProfileRepository resumeRepository,
            ResumeParserService parserService,
            ILogger<ResumeController> logger)
        {
            _resumeRepository = resumeRepository;
            _parserService = parserService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<UploadResumeResponse>> UploadResume(IFormFile file)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                // Validate file
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var allowedExtensions = new[] { ".pdf", ".docx" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("Only PDF and DOCX files are supported");
                }

                if (file.Length > 5 * 1024 * 1024) // 5MB limit
                {
                    return BadRequest("File size must be less than 5MB");
                }

                // Parse resume using Python service
                ResumeMetadata? metadata;
                using (var stream = file.OpenReadStream())
                {
                    metadata = await _parserService.ParseResumeAsync(stream, file.FileName);
                }

                if (metadata == null)
                {
                    return BadRequest("Failed to parse resume. Please ensure the file is a valid resume.");
                }

                // For simplicity, store resume URL as filename (in production, upload to cloud storage)
                // You would integrate Cloudinary or similar here
                var resumeUrl = $"/resumes/{userId}_{file.FileName}";

                // Create resume profile
                var resumeProfile = new ResumeProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FileName = file.FileName,
                    ResumeUrl = resumeUrl,
                    FileType = extension.TrimStart('.'),
                    Skills = string.Join(", ", metadata.Skills),
                    Experience = metadata.Experience,
                    Education = string.Join("; ", metadata.Education),
                    JobTitles = string.Join("; ", metadata.JobTitles),
                    Technologies = string.Join(", ", metadata.Technologies),
                    UploadedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var created = await _resumeRepository.CreateAsync(resumeProfile);

                var response = new UploadResumeResponse
                {
                    ResumeId = created.Id,
                    FileName = created.FileName,
                    Metadata = new ResumeMetadataDto
                    {
                        Skills = metadata.Skills,
                        Experience = metadata.Experience,
                        Education = metadata.Education,
                        JobTitles = metadata.JobTitles,
                        Technologies = metadata.Technologies,
                        Summary = metadata.Summary
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading resume: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your resume");
            }
        }

        [HttpGet("my-resumes")]
        public async Task<ActionResult<List<ResumeProfileResponse>>> GetMyResumes()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var resumes = await _resumeRepository.GetByUserIdAsync(userId);

                var response = resumes.Select(r => new ResumeProfileResponse
                {
                    Id = r.Id,
                    FileName = r.FileName,
                    Experience = r.Experience,
                    Skills = r.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToList(),
                    UploadedAt = r.UploadedAt
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching resumes: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching your resumes");
            }
        }

        [HttpGet("latest")]
        public async Task<ActionResult<ResumeProfileResponse>> GetLatestResume()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var resume = await _resumeRepository.GetLatestByUserIdAsync(userId);

                if (resume == null)
                {
                    return NotFound("No resume found");
                }

                var response = new ResumeProfileResponse
                {
                    Id = resume.Id,
                    FileName = resume.FileName,
                    Experience = resume.Experience,
                    Skills = resume.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToList(),
                    UploadedAt = resume.UploadedAt
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching latest resume: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching your resume");
            }
        }
    }
}
