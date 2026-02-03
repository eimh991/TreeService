using Microsoft.AspNetCore.Mvc;
using TreeService.DTOs;
using TreeService.Services;

namespace TreeService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Regiister(RegisterDto dto , CancellationToken cancellationToken)
        {
            await _authService.RegistrAsync(dto, cancellationToken);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto,CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(dto, cancellationToken);

            return Ok(new {token});
        }
    }
}
