using System.Linq;
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
                Email = userEntity.Email,
                GivenName = userEntity.GivenName,
                FamilyName = userEntity.FamilyName,
                DateRegisteredUtc = userEntity.DateRegisteredUtc,
                FileItems = userEntity.FileItems?.Select(x => x.ToFileItem())
            };
        }

        public static UserEntity ToUserEntity(this User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Email = user.Email,
                GivenName = user.GivenName,
                FamilyName = user.FamilyName,
                DateRegisteredUtc = user.DateRegisteredUtc,
                FileItems = user.FileItems?.Select(x => x.ToFileItemEntity()).ToList()
            };
        }
    }
}
