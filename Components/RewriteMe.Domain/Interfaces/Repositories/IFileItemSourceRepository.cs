using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemSourceRepository
    {
        FileItemSource GetFileItemSource(Guid fileItemId);

        Task<bool> HasFileItemSourceAsync(Guid fileItemId);

        Task AddAsync(FileItemSource fileItemSource);

        Task UpdateSourceAsync(Guid fileItemId, byte[] source);

        Task RemoveAsync(Guid fileItemId);
    }
}
