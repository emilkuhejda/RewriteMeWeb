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
        private const string ChunksDirectory = "chunks";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileAccessService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetRootPath()
        {
            var rootDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, UploadedFilesDirectory);
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

        public string GetChunksStoragePath()
        {
            var rootDirectory = GetRootPath();
            var directoryPath = Path.Combine(rootDirectory, ChunksDirectory);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        public string GetChunksFileItemStoragePath(Guid fileItemId)
        {
            var rootDirectory = GetChunksStoragePath();
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }
    }
}
