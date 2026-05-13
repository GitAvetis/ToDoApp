using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Infrastructure.Entitys;

namespace ToDoWebApplication.Infrastructure.Mappers
{
    internal class UserEntityMapper
    {
        public static UserEntity ToUserEntity(UserModel user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.PasswordHash
            };
        }

        public static UserModel ToUserModel(UserEntity entity)
        {
            return UserModel.Restore(entity.Id, entity.Login, entity.Password);
        }
    }
}
