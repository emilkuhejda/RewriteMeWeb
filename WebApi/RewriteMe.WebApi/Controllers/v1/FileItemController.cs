using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/files")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class FileItemController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public FileItemController(
            IFileItemService fileItemService,
            ISpeechRecognitionManager speechRecognitionManager,
            IMediator mediator,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
            _mediator = mediator;
            _logger = logger.ForContext<FileItemController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetFileItems")]
        public async Task<IActionResult> Get(DateTime updatedAfter, Guid applicationId)
        {
            _logger.Information($"Retrieve data for minimum date '{updatedAfter}'.");

            var userId = HttpContext.User.GetNameIdentifier();
            var files = await _fileItemService.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);

            return Ok(files.Select(x => x.ToDto()));
        }

        [HttpGet("deleted")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetDeletedFileItemIds")]
        public async Task<IActionResult> GetDeletedFileItemIds(DateTime updatedAfter, Guid applicationId)
        {
            _logger.Information($"Retrieve data for minimum date '{updatedAfter}'.");

            var userId = HttpContext.User.GetNameIdentifier();
            var ids = await _fileItemService.GetAllDeletedIdsAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);

            return Ok(ids);
        }

        [HttpGet("temporary-deleted")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetTemporaryDeletedFileItems()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var fileItems = await _fileItemService.GetTemporaryDeletedFileItemsAsync(userId).ConfigureAwait(false);

            return Ok(fileItems);
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetFileItem")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var file = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(file.ToDto());
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateFileItem")]
        public async Task<IActionResult> CreateFileItem(string name, string language, string fileName, DateTime dateCreated, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var createFileItemCommand = new CreateFileItemCommand
            {
                UserId = userId,
                Name = name,
                Language = language,
                FileName = fileName,
                DateCreated = dateCreated,
                ApplicationId = applicationId
            };

            var fileItemDto = await _mediator.Send(createFileItemCommand).ConfigureAwait(false);
            return Ok(fileItemDto);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UploadFileItem")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Upload(string name, string language, string fileName, DateTime dateCreated, Guid applicationId, IFormFile file)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var uploadFileSourceCommand = new UploadFileSourceCommand
            {
                UserId = userId,
                Name = name,
                Language = language,
                FileName = fileName,
                DateCreated = dateCreated,
                ApplicationId = applicationId,
                File = file
            };

            var fileItemDto = await _mediator.Send(uploadFileSourceCommand).ConfigureAwait(false);
            return Ok(fileItemDto);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateFileItem")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Update([FromForm]UpdateFileItemModel updateFileItemModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var updateFileItemCommand = new UpdateFileItemCommand
            {
                UserId = userId,
                FileItemId = updateFileItemModel.FileItemId,
                Name = updateFileItemModel.Name,
                Language = updateFileItemModel.Language,
                ApplicationId = updateFileItemModel.ApplicationId
            };

            var fileItemDto = await _mediator.Send(updateFileItemCommand).ConfigureAwait(false);
            return Ok(fileItemDto);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteFileItem")]
        public async Task<IActionResult> Delete(Guid fileItemId, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var deleteFileItemCommand = new DeleteFileItemCommand
            {
                UserId = userId,
                FileItemId = fileItemId,
                ApplicationId = applicationId
            };

            var okDto = await _mediator.Send(deleteFileItemCommand).ConfigureAwait(false);

            return Ok(okDto);
        }

        [HttpDelete("delete-all")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteAllFileItems")]
        public async Task<IActionResult> DeleteAll(IEnumerable<DeletedFileItemModel> fileItems, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var deleteFileItemsCommand = new DeleteFileItemsCommand
            {
                UserId = userId,
                FileItems = fileItems,
                ApplicationId = applicationId
            };

            var okDto = await _mediator.Send(deleteFileItemsCommand).ConfigureAwait(false);

            return Ok(okDto);
        }

        [HttpPut("permanent-delete-all")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PermanentDeleteAll(IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var permanentDeleteAllCommand = new PermanentDeleteAllCommand
            {
                UserId = userId,
                FileItemIds = fileItemIds,
                ApplicationId = applicationId
            };

            var okDto = await _mediator.Send(permanentDeleteAllCommand).ConfigureAwait(false);

            return Ok(okDto);
        }

        [HttpPut("restore-all")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RestoreAll(IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var restoreAllCommand = new RestoreAllCommand
            {
                UserId = userId,
                FileItemIds = fileItemIds,
                ApplicationId = applicationId
            };

            var okDto = await _mediator.Send(restoreAllCommand).ConfigureAwait(false);

            return Ok(okDto);
        }

        [HttpPut("transcribe")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "TranscribeFileItem")]
        public async Task<IActionResult> Transcribe(Guid fileItemId, string language, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var canRunRecognitionCommand = new CanRunRecognitionCommand
            {
                UserId = userId,
                FileItemId = fileItemId,
                Language = language,
                ApplicationId = applicationId
            };

            var okDto = await _mediator.Send(canRunRecognitionCommand).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognitionAsync(userId, fileItemId));

            return Ok(okDto);
        }
    }
}