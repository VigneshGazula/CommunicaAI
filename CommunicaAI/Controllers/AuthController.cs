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

    public AuthController(ApplicationDbContext context, ITokenService tokenService, IPasswordHasher<AppUser> passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
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

        var(token, expiresAtUtc) = _tokenService.CreateToken(user);

        var response = new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };
        return Ok(response);

    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
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

        var response = new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };

        return Ok(response);

    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Message = "Authenticated",
            User = User.Identity?.Name
        });
    }

}
