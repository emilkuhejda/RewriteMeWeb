using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class DeletedAccountDataAdapter
    {
        public static DeletedAccount ToDeletedAccount(this DeletedAccountEntity deletedAccountEntity)
        {
            return new DeletedAccount
            {
                Id = deletedAccountEntity.Id,
                UserId = deletedAccountEntity.UserId
            };
        }

        public static DeletedAccountEntity ToDeletedAccountEntity(this DeletedAccount deletedAccount)
        {
            return new DeletedAccountEntity
            {
                Id = deletedAccount.Id,
                UserId = deletedAccount.UserId
            };
        }
    }
}
