using System;

namespace RewriteMe.DataAccess
{
    public interface IDbContextFactory
    {
        AppDbContext Create();

        AppDbContext Create(TimeSpan timeout);
    }
}
