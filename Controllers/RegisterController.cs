using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class RegisterController (IAuthService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        try
        {
            var registerResponse = await service.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { id = registerResponse.Id }, registerResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}