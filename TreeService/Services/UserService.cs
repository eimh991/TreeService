using Microsoft.EntityFrameworkCore;
using TreeService.Auth;
using TreeService.Data;
using TreeService.DTOs;

namespace TreeService.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Метод смены роли с user на admin, я не стал делать лишнии репозитории, просто что бы не тратить время.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ChangeRoleAsync(ChangeUserRoleDto dto, CancellationToken cancellationToken)
        {
            if(dto.Role != Roles.Admin && dto.Role != Roles.User)
            {
                throw new Exception("Нету такой роли");
            }

            var user = await _db.Users.
                FirstOrDefaultAsync(u=>u.Id == dto.UserId, cancellationToken);

            if (user == null) 
            {
                throw new Exception("Пользователь не найден");
            }

            user.Role = dto.Role;

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
