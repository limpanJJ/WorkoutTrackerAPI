using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos.Auth.Requests;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var loginResponse = await authService.LoginAsync(request);
        return Ok(loginResponse);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        var registerResponse = await authService.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), new { id = registerResponse.Id }, registerResponse);
    }
}