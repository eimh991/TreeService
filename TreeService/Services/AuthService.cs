using Microsoft.EntityFrameworkCore;
using TreeService.Auth;
using TreeService.Data;
using TreeService.DTOs;
using TreeService.Entities;

namespace TreeService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(AppDbContext db, IPasswordHasher hasher, IJwtTokenService jwtTokenService)
        {
            _db = db;
            _hasher = hasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task RegistrAsync(RegisterDto dto, CancellationToken cancellationToken)
        {
            var exists = await _db.Users
                .AnyAsync(u => u.UserName == dto.Username, cancellationToken);

            if (exists)
            {
                throw new Exception("Пользователь уже существует");
            }

            var user = new User
            {
                UserName = dto.Username,
                PasswordHash = _hasher.Hash(dto.Password),
                Role = Roles.User
            }; 

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.Username,cancellationToken);

            if(user == null || !_hasher.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Неверный логин или пароль");

            }
            return _jwtTokenService.GenerateToken(user);
        }

        
    }
}
