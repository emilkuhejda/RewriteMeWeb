using System.Threading.Tasks;
using Moq;
using RewriteMe.Business.Services;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using Xunit;

namespace RewriteMe.Business.Tests
{
    public class FileItemServiceTests
    {
        private readonly IFileItemService _fileItemService;
        private readonly Mock<IFileItemSourceService> _fileItemSourceServiceMock;
        private readonly Mock<IInternalValueService> _internalValueServiceMock;
        private readonly Mock<IFileAccessService> _fileAccessServiceMock;
        private readonly Mock<IFileItemRepository> _fileItemRepositoryMock;

        public FileItemServiceTests()
        {
            _fileItemSourceServiceMock = new Mock<IFileItemSourceService>();
            _internalValueServiceMock = new Mock<IInternalValueService>();
            _fileAccessServiceMock = new Mock<IFileAccessService>();
            _fileItemRepositoryMock = new Mock<IFileItemRepository>();

            _fileItemService = new FileItemService(
                _fileItemSourceServiceMock.Object,
                _internalValueServiceMock.Object,
                _fileAccessServiceMock.Object,
                _fileItemRepositoryMock.Object);
        }

        [Trait("Category", "Unit")]
        [Fact]
        public async Task GetAudioSource_ReturnsSourceFromDatabase()
        { }
    }
}
