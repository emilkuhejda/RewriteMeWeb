using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class DeletedUserDataAdapter
    {
        public static DeletedUser ToDeletedUser(this DeletedUserEntity deletedUserEntity)
        {
            return new DeletedUser
            {
                Id = deletedUserEntity.Id,
                UserId = deletedUserEntity.UserId
            };
        }

        public static DeletedUserEntity ToDeletedUserEntity(this DeletedUser deletedUser)
        {
            return new DeletedUserEntity
            {
                Id = deletedUser.Id,
                UserId = deletedUser.UserId
            };
        }
    }
}
