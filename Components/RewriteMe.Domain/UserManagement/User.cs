using System;
using System.Collections.Generic;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.UserManagement
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public IEnumerable<FileItem> FileItems { get; set; }
    }
}
