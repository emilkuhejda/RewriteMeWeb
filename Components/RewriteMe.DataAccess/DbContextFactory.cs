using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Settings;

namespace RewriteMe.DataAccess
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly AppSettings _appSettings;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public DbContextFactory(
            IOptions<AppSettings> options,
            DbContextOptions<AppDbContext> contextOptions)
        {
            _appSettings = options.Value;
            _contextOptions = contextOptions;
        }

        public AppDbContext Create()
        {
            return new AppDbContext(_appSettings.ConnectionString, _contextOptions);
        }
    }
}
