using System;

namespace RewriteMe.Domain.UserManagement
{
    public class DeletedAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateDeleted { get; set; }
    }
}
