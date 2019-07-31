using System.IO;
using Microsoft.AspNetCore.Hosting;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Services
{
    public class HostingEnvironmentService : IHostingEnvironmentService
    {
        private const string UploadedFilesDirectory = "uploaded";

        private readonly IHostingEnvironment _hostingEnvironment;

        public HostingEnvironmentService(IHostingEnvironment hostingEnvironment)
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
    }
}
