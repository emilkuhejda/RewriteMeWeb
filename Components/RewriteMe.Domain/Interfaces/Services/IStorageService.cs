using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IStorageService
    {
        Task MigrateAsync();

        Task<byte[]> GetFileItemBytesAsync(FileItem fileItem);

        Task<byte[]> GetTranscribeItemBytesAsync(TranscribeItem transcribeItem, Guid userId);

        Task DeleteFileItemSourceAsync(FileItem fileItem);

        Task DeleteContainerAsync(Guid userId);
    }
}
