using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class LoginController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            try
            {
                var loginResponse = await authService.LoginAsync(request);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
