using System;

namespace RewriteMe.WebApi.Models
{
    public class RegisterUserModel
    {
        public Guid Id { get; set; }

        public Guid ApplicationId { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }
    }
}
