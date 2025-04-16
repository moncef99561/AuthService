using AuthService.Services;
using AuthService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("external-register")]
        public async Task<IActionResult> Register([FromBody] RegisterFromExternalVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authService.RegisterAsync(vm);
            if (!success)
                return Conflict(new { message = "Nom d'utilisateur déjà utilisé." });

            return Ok(new { message = "Compte enregistré dans AuthService." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM vm)
        {
            var (token, error) = await _authService.LoginAsync(vm);

            if (error != null)
                return Unauthorized(new { message = error });

            return Ok(new { token });
        }
    }
}
