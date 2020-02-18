using System;
using System.IO;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileAccessService
    {
        string GetRootPath(Guid userId);

        string GetFileItemDirectory(Guid userId, Guid fileItemId);

        string GetOriginalFileItemPath(FileItem fileItem);

        string GetFileItemPath(FileItem fileItem);

        string GetTranscriptionsDirectoryPath(Guid userId, Guid fileItemId);

        string GetTranscriptionPath(Guid userId, TranscribeItem transcribeItem);

        DirectoryInfo GetFileItemDirectoryInfo(Guid userId, Guid fileItemId);

        string GetChunksFileItemStoragePath(Guid fileItemId);

        string GetChunksStoragePath();
    }
}
