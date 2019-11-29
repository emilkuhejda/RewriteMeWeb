using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemSourceService
    {
        Task<FileItemSource> GetAsync(Guid fileItemId);

        Task<bool> HasFileItemSourceAsync(Guid fileItemId);

        Task AddFileItemSourceAsync(FileItem fileItem, string fileItemPath);

        Task UpdateSourceAsync(Guid fileItemId, byte[] source);
    }
}
