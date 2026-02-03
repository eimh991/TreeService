using TreeService.DTOs;

namespace TreeService.Services
{
    public interface IUserService
    {
        Task ChangeRoleAsync(ChangeUserRoleDto dto, CancellationToken cancellationToken);
    }
}
