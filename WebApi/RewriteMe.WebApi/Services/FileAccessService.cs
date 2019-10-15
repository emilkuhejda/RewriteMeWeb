using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Services
{
    public class FileAccessService : IFileAccessService
    {
        private const string UploadedFilesDirectory = "uploaded";
        private const string TranscriptionsDirectory = "transcriptions";

        private readonly IHostingEnvironment _hostingEnvironment;

        public FileAccessService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetRootPath()
        {
            var rootDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, UploadedFilesDirectory);
            if (!Directory.Exists(rootDirectoryPath))
                Directory.CreateDirectory(rootDirectoryPath);

            return rootDirectoryPath;
        }

        public string GetFileItemDirectory(Guid fileItemId)
        {
            return Path.Combine(GetRootPath(), fileItemId.ToString());
        }

        public string GetOriginalFileItemPath(FileItem fileItem)
        {
            var rootDirectory = GetRootPath();
            return Path.Combine(rootDirectory, fileItem.OriginalSourcePath);
        }

        public string GetFileItemPath(FileItem fileItem)
        {
            var rootDirectory = GetRootPath();
            return Path.Combine(rootDirectory, fileItem.SourcePath);
        }

        public string GetTranscriptionsDirectoryPath(Guid fileItemId)
        {
            var rootDirectory = GetRootPath();
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString(), TranscriptionsDirectory);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        public string GetTranscriptionPath(TranscribeItem transcribeItem)
        {
            var directoryPath = GetTranscriptionsDirectoryPath(transcribeItem.FileItemId);

            return Path.Combine(directoryPath, transcribeItem.SourceFileName);
        }

        public DirectoryInfo GetFileItemDirectoryInfo(Guid fileItemId)
        {
            var rootDirectory = GetRootPath();
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString());

            return new DirectoryInfo(directoryPath);
        }
    }
}
