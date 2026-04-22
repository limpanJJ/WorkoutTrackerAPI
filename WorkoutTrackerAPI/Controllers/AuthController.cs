using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos.Auth.Requests;
using WorkoutTrackerAPI.Dtos.Auth.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;
/// <summary>Endpoints for user authentication and registration.</summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Logs in a user.</summary>
    /// <response code="200">Login successful.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">Invalid credentials.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var loginResponse = await authService.LoginAsync(request);
        return Ok(loginResponse);
    }
    /// <summary>Registers a new user.</summary>
    /// <response code="201">Registration successful.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="409">Email or username already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var registerResponse = await authService.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), new { id = registerResponse.Id }, registerResponse);
    }
}