using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RewriteMe.DataAccess
{
    public class AppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var connectionString = configuration.GetSection("ApplicationSettings").GetSection("ConnectionString").Value;

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            return new AppDbContext(connectionString, optionsBuilder.Options);
        }
    }
}
