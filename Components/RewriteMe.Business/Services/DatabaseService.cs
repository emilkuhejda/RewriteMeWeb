using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly ILogger _logger;

        public DatabaseService(
            IDatabaseRepository databaseRepository,
            ILogger logger)
        {
            _databaseRepository = databaseRepository;
            _logger = logger.ForContext<DatabaseService>();
        }

        public async Task ResetAsync()
        {
            await _databaseRepository.ResetAsync().ConfigureAwait(false);

            _logger.Information("Database was reset.");
        }

        public async Task DeleteDatabaseAsync()
        {
            await _databaseRepository.DeleteDatabaseAsync().ConfigureAwait(false);

            _logger.Information("Database was deleted.");
        }
    }
}
