using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Logging;

namespace RewriteMe.Business.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository _applicationLogRepository;

        public ApplicationLogService(IApplicationLogRepository applicationLogRepository)
        {
            _applicationLogRepository = applicationLogRepository;
        }

        public async Task Add(ApplicationLog applicationLog)
        {
            await _applicationLogRepository.Add(applicationLog).ConfigureAwait(false);
        }
    }
}
