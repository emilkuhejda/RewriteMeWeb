using System;
using System.Collections.Generic;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.UserManagement
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public DateTime DateRegistered { get; set; }

        public IEnumerable<FileItem> FileItems { get; set; }
    }
}
