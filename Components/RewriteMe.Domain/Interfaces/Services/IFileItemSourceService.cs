using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemSourceService
    {
        Task<FileItemSource> GetAsync(Guid fileItemId);

        Task AddFileItemSourceAsync(FileItem fileItem);

        Task UpdateSource(Guid fileItemId, byte[] source);
    }
}
