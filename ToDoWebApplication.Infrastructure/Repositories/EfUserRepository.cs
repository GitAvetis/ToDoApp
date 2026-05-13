using Microsoft.EntityFrameworkCore;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Infrastructure.Entitys;
using ToDoWebApplication.Infrastructure.Mappers;
using ToDoWebApplication.Infrastructure.Persistence;

namespace ToDoWebApplication.Infrastructure.Repositories
{
    public class EfUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public EfUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserModel user)
        {
            UserEntity entity = UserEntityMapper.ToUserEntity(user);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel?> GetByLoginAsync(string login)
        {
            UserEntity user = await _context.Users
                .FirstOrDefaultAsync(user => user.Login == login);
            if (user == null)
            {
                return null;
            }

            return UserEntityMapper.ToUserModel(user);
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            UserEntity user = await _context.Users
                .FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                return null;
            }

            return UserEntityMapper.ToUserModel(user);
        }

    }
}
