using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemSourceRepository
    {
        Task<FileItemSource> GetAsync(Guid fileItemId);

        Task<bool> HasFileItemSourceAsync(Guid fileItemId);

        Task AddAsync(FileItemSource fileItemSource);

        Task UpdateSource(Guid fileItemId, byte[] source);
    }
}
