using System;
using Microsoft.EntityFrameworkCore.Internal;
using RewriteMe.Common.Helpers;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.Seed
{
    public static class DbSeed
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Administrators.Any())
                return;

            var password = PasswordHelper.CreateHash("VoicipherPass12!");
            var administrator = new AdministratorEntity
            {
                Id = Guid.NewGuid(),
                Username = "emil.kuhejda@gmail.com",
                FirstName = "Emil",
                LastName = "Kuhejda",
                PasswordHash = password.PasswordHash,
                PasswordSalt = password.PasswordSalt
            };

            context.Administrators.Add(administrator);
            context.SaveChanges();
        }
    }
}
