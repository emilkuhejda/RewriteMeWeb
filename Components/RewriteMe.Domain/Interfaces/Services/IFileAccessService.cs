using System;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileAccessService
    {
        string GetRootPath(Guid userId);

        string GetFileItemRootDirectory(Guid userId, Guid fileItemId);

        string GetFileItemSourceDirectory(Guid userId, Guid fileItemId);

        string GetOriginalFileItemPath(FileItem fileItem);

        string GetFileItemPath(FileItem fileItem);

        string GetTranscriptionsDirectoryPath(Guid userId, Guid fileItemId);

        string GetTranscriptionPath(Guid userId, TranscribeItem transcribeItem);

        string GetChunksFileItemStoragePath(Guid fileItemId);

        string GetChunksStoragePath();

        string GetPartialFilesDirectoryPath(Guid userId, Guid fileItemId);
    }
}
