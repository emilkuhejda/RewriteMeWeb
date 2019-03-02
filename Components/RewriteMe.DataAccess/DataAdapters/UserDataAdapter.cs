using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class UserDataAdapter
    {
        public static User ToUser(this UserEntity userEntity)
        {
            return new User
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                PasswordHash = userEntity.PasswordHash,
                PasswordSalt = userEntity.PasswordSalt
            };
        }

        public static UserEntity ToUserEntity(this User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
        }
    }
}
