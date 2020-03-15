using System.Threading.Tasks;
using Moq;
using RewriteMe.Business.Services;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using Serilog;
using Xunit;

namespace RewriteMe.Business.Tests
{
    public class FileItemServiceTests
    {
        private readonly IFileItemService _fileItemService;
        private readonly Mock<IFileItemSourceService> _fileItemSourceServiceMock;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly Mock<IInternalValueService> _internalValueServiceMock;
        private readonly Mock<IFileAccessService> _fileAccessServiceMock;
        private readonly Mock<IApplicationLogService> _applicationLogServiceMock;
        private readonly Mock<IFileItemRepository> _fileItemRepositoryMock;
        private readonly Mock<IFileItemSourceRepository> _fileItemSourceRepositoryMock;
        private readonly Mock<ILogger> _loggerMock;

        public FileItemServiceTests()
        {
            _fileItemSourceServiceMock = new Mock<IFileItemSourceService>();
            _internalValueServiceMock = new Mock<IInternalValueService>();
            _storageServiceMock = new Mock<IStorageService>();
            _fileAccessServiceMock = new Mock<IFileAccessService>();
            _applicationLogServiceMock = new Mock<IApplicationLogService>();
            _fileItemRepositoryMock = new Mock<IFileItemRepository>();
            _fileItemSourceRepositoryMock = new Mock<IFileItemSourceRepository>();
            _loggerMock = new Mock<ILogger>();

            _fileItemService = new FileItemService(
                _fileItemSourceServiceMock.Object,
                _storageServiceMock.Object,
                _internalValueServiceMock.Object,
                _fileAccessServiceMock.Object,
                _fileItemRepositoryMock.Object,
                _fileItemSourceRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Trait("Category", "Unit")]
        [Fact]
        public async Task GetAudioSource_ReturnsSourceFromDatabase()
        { }
    }
}
