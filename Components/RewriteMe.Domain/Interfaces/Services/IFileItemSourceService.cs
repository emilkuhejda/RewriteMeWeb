using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemSourceService
    {
        Task AddFileItemSourceAsync(FileItem fileItem);
    }
}
