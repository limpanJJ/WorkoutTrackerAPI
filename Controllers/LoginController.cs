using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos.Auth.Requests;
using WorkoutTrackerAPI.Routes;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers
{
    [Route(ApiRoutes.Login.Base)]
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
