using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Handlers
{
    public class MigrateFilesHandler : IRequestHandler<MigrateFilesCommand, OkDto>
    {
        private readonly IStorageService _storageService;

        public MigrateFilesHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<OkDto> Handle(MigrateFilesCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return new OkDto();
        }
    }
}
