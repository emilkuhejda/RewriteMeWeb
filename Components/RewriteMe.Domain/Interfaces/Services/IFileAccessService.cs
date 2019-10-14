using System;
using System.IO;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileAccessService
    {
        string GetRootPath();

        string GetFileItemDirectory(Guid fileItemId);

        string GetOriginalFileItemPath(FileItem fileItem);

        string GetFileItemPath(FileItem fileItem);

        string GetTranscriptionsDirectoryPath(Guid fileItemId);

        string GetTranscriptionPath(TranscribeItem transcribeItem);

        DirectoryInfo GetFileItemDirectoryInfo(Guid fileItemId);
    }
}
