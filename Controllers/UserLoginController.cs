using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers
{
    [Route("api/auth/login")]
    [ApiController]
    public class UserLoginController(IAuthService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            try
            {
                var userResponse = await service.LoginAsync(request);
                return CreatedAtAction(nameof(Login), new { id = userResponse.Id }, userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
