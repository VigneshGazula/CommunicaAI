using CommunicaAI.Data;
using CommunicaAI.DTO.Auth;
using CommunicaAI.Models;
using CommunicaAI.Services;
using CommunicaAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommunicaAI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public AuthController(ApplicationDbContext context, ITokenService tokenService, IPasswordHasher<AppUser> passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
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

    [HttpPost("login/password")]
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

    [HttpPost("login/guest")]
    public async Task<ActionResult<AuthResponse>> LoginAsGuest()
    {
        var guestId = Guid.NewGuid().ToString("N")[..8];
        var fullName = $"Guest-{guestId}";
        var email = $"guest-{guestId}@communicaai.app";
        var tempPassword = Guid.NewGuid().ToString();

        var user = new AppUser
        {
            FullName = fullName,
            Email = email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, tempPassword);
        _context.Add(user);
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

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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