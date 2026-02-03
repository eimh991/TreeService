using Microsoft.AspNetCore.Mvc;
using TreeService.DTOs;
using TreeService.Services;

namespace TreeService.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // [Authorize(Roles = Roles.Admin)] - установить ограничение после того как появится хотя бы 1 админ. 
        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeRole(
            ChangeUserRoleDto dto,
            CancellationToken cancellationToken)
        {
            await _userService.ChangeRoleAsync(dto, cancellationToken);
            return NoContent();
        }
    }
}
