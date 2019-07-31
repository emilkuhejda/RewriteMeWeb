using System;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileAccessService
    {
        string GetRootPath();

        string GetOriginalFileItemPath(FileItem fileItem);

        string GetFileItemPath(FileItem fileItem);

        string GetTranscriptionsDirectoryPath(Guid fileItemId);

        string GetTranscriptionPath(TranscribeItem transcribeItem);
    }
}
