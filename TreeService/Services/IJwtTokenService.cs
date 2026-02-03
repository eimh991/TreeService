using TreeService.Entities;

namespace TreeService.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
