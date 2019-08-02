using System;

namespace RewriteMe.Domain.UserManagement
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public Guid ApplicationId { get; set; }

        public DateTime DateRegistered { get; set; }
    }
}
