using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/auth/register")]
[ApiController]
public class UserRegisterController (IAuthService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        try
        {
            var userResponse = await service.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { id = userResponse.Id }, userResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}