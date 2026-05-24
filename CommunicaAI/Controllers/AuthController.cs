using CommunicaAI.Data;
using CommunicaAI.DTO.Auth;
using CommunicaAI.Models;
using CommunicaAI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunicaAI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IBiometricVerificationService _biometricVerificationService;
    private readonly IPythonVerificationService _pythonVerificationService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService, IPasswordHasher<AppUser> passwordHasher, ICloudinaryService cloudinaryService, IBiometricVerificationService biometricVerificationService, IPythonVerificationService pythonVerificationService)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _cloudinaryService = cloudinaryService;
        _biometricVerificationService = biometricVerificationService;
        _pythonVerificationService = pythonVerificationService;
    }

    [HttpPost("register")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLower();
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if(existingUser!=null)
        {
            return Conflict(new { message = "Email already Registered" });
        }

        var user = new AppUser
        {
            FullName = request.FullName.Trim(),
            Email = email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user,request.Password);
        _context.Add(user);
        await _context.SaveChangesAsync();

        var audioUpload = await _cloudinaryService.UploadAudioAsync(request.AudioFile, user.Id);
        var videoUpload = await _cloudinaryService.UploadVideoAsync(request.VideoFile, user.Id);

        var profile = new UserVerificationProfile
        {
            UserId = user.Id,
            EnrollmentAudioUrl = audioUpload.Url,
            EnrollmentAudioPublicId = audioUpload.PublicId,
            EnrollmentVideoUrl = videoUpload.Url,
            EnrollmentVideoPublicId = videoUpload.PublicId,
            EnrolledAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        _context.UserVerificationProfiles.Add(profile);
        await _context.SaveChangesAsync();

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        });
    }

    [HttpPost("login/passoword")]
    public async Task<ActionResult<AuthResponse>> LoginWithPassword([FromBody] PasswordLoginRequest request)
    {
        var email = request.Email.Trim().ToLower();
        AppUser user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid email or password." });

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        });
    }

    [HttpPost("login/audio")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuthResponse>> LoginWithAudio([FromForm] AudioLoginRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (user == null)
            return Unauthorized(new { message = "Invalid email or audio verification failed." });

        var profile = await _context.UserVerificationProfiles
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (profile == null || string.IsNullOrWhiteSpace(profile.EnrollmentAudioUrl))
            return Unauthorized(new { message = "Audio verification not enrolled." });

        PythonVerificationResult verificationResult;

        try
        {
            verificationResult = await _pythonVerificationService.VerifyAudioAsync(
                profile.EnrollmentAudioUrl,
                request.AudioFile);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "Verification service unavailable.",
                detail = ex.Message
            });
        }

        if (!verificationResult.Verified)
            return Unauthorized(new { message = "Invalid email or audio verification failed." });

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        });
    }

    [HttpPost("login/video")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuthResponse>> LoginWithVideo([FromForm] VideoLoginRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (user == null)
            return Unauthorized(new { message = "Invalid email or video verification failed." });

        var profile = await _context.UserVerificationProfiles
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (profile == null)
            return Unauthorized(new { message = "Video verification not enrolled." });

        var isVerified = await _biometricVerificationService.VerifyVideoAsync(profile, request.VideoFile);

        if (!isVerified)
            return Unauthorized(new { message = "Invalid email or video verification failed." });

        var (token, expiresAtUtc) = _tokenService.CreateToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userIdClaim = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token." });

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email
        });
    }

}
