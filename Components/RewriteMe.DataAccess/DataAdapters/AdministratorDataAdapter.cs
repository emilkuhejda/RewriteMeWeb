using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Administration;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class AdministratorDataAdapter
    {
        public static Administrator ToAdministrator(this AdministratorEntity administratorEntity)
        {
            return new Administrator
            {
                Id = administratorEntity.Id,
                Username = administratorEntity.Username,
                FirstName = administratorEntity.FirstName,
                LastName = administratorEntity.LastName,
                PasswordHash = administratorEntity.PasswordHash,
                PasswordSalt = administratorEntity.PasswordSalt
            };
        }

        public static AdministratorEntity ToAdministratorEntity(this Administrator administrator)
        {
            return new AdministratorEntity
            {
                Id = administrator.Id,
                Username = administrator.Username,
                FirstName = administrator.FirstName,
                LastName = administrator.LastName,
                PasswordHash = administrator.PasswordHash,
                PasswordSalt = administrator.PasswordSalt
            };
        }
    }
}
