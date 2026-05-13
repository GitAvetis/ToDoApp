using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(UserModel user);
        Task<UserModel?> GetByIdAsync(Guid id);
        Task<UserModel?> GetByLoginAsync(string login);
    }
}
