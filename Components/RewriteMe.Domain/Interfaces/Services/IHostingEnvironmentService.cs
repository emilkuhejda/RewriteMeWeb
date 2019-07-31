using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IHostingEnvironmentService
    {
        string GetRootPath();

        string GetOriginalFileItemPath(FileItem fileItem);

        string GetFileItemPath(FileItem fileItem);
    }
}
