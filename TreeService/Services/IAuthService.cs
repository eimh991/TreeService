using TreeService.DTOs;
using TreeService.Entities;

namespace TreeService.Services
{
    public interface IAuthService
    {
        Task RegistrAsync(RegisterDto dto, CancellationToken cancellationToken);
        Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    }
}
