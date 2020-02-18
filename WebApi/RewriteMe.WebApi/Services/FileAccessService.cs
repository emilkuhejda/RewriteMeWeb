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

        public string GetRootPath(Guid userId)
        {
            var rootDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, UploadedFilesDirectory, userId.ToString());
            if (!Directory.Exists(rootDirectoryPath))
                Directory.CreateDirectory(rootDirectoryPath);

            return rootDirectoryPath;
        }

        public string GetFileItemDirectory(Guid userId, Guid fileItemId)
        {
            return Path.Combine(GetRootPath(userId), fileItemId.ToString());
        }

        public string GetOriginalFileItemPath(FileItem fileItem)
        {
            var rootDirectory = GetRootPath(fileItem.UserId);
            return Path.Combine(rootDirectory, fileItem.OriginalSourcePath);
        }

        public string GetFileItemPath(FileItem fileItem)
        {
            var rootDirectory = GetRootPath(fileItem.UserId);
            return Path.Combine(rootDirectory, fileItem.SourcePath);
        }

        public string GetTranscriptionsDirectoryPath(Guid userId, Guid fileItemId)
        {
            var rootDirectory = GetRootPath(userId);
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString(), TranscriptionsDirectory);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        public string GetTranscriptionPath(Guid userId, TranscribeItem transcribeItem)
        {
            var directoryPath = GetTranscriptionsDirectoryPath(userId, transcribeItem.FileItemId);

            return Path.Combine(directoryPath, transcribeItem.SourceFileName);
        }

        public DirectoryInfo GetFileItemDirectoryInfo(Guid userId, Guid fileItemId)
        {
            var rootDirectory = GetRootPath(userId);
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString());

            return new DirectoryInfo(directoryPath);
        }

        public string GetChunksFileItemStoragePath(Guid fileItemId)
        {
            var rootDirectory = GetChunksStoragePath();
            var directoryPath = Path.Combine(rootDirectory, fileItemId.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        public string GetChunksStoragePath()
        {
            var rootDirectory = GetRootPath();
            var directoryPath = Path.Combine(rootDirectory, ChunksDirectory);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        private string GetRootPath()
        {
            var rootDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, UploadedFilesDirectory);
            if (!Directory.Exists(rootDirectoryPath))
                Directory.CreateDirectory(rootDirectoryPath);

            return rootDirectoryPath;
        }
    }
}
