using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RewriteMe.Business.Configuration;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/chunks")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class UploadedChunkController : ControllerBase
    {
        private readonly IUploadedChunkService _uploadedChunkService;
        private readonly IInternalValueService _internalValueService;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UploadedChunkController(
            IUploadedChunkService uploadedChunkService,
            IInternalValueService internalValueService,
            IMediator mediator,
            ILogger logger)
        {
            _uploadedChunkService = uploadedChunkService;
            _internalValueService = internalValueService;
            _mediator = mediator;
            _logger = logger.ForContext<UploadedChunkController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(StorageConfiguration), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetChunksStorageConfiguration")]
        public async Task<IActionResult> GetChunksStorageConfiguration()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.ChunksStorageSetting).ConfigureAwait(false);
            var configuration = new StorageConfiguration
            {
                StorageSetting = storageSetting
            };


            _logger.Information($"Retrieve storage configuration: {JsonConvert.SerializeObject(configuration)}. [{userId}]");

            return Ok(configuration);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UploadChunkFile")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Upload(Guid fileItemId, int order, StorageSetting storageSetting, Guid applicationId, IFormFile file, CancellationToken cancellationToken)
        {
            var uploadChunkFileCommand = new UploadChunkFileCommand
            {
                FileItemId = fileItemId,
                Order = order,
                StorageSetting = storageSetting,
                ApplicationId = applicationId,
                File = file
            };

            var okDto = await _mediator.Send(uploadChunkFileCommand, cancellationToken).ConfigureAwait(false);
            return Ok(okDto);
        }

        [HttpPut("submit")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "SubmitChunks")]
        public async Task<IActionResult> Submit(Guid fileItemId, int chunksCount, StorageSetting chunksStorageSetting, Guid applicationId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var submitChunksCommand = new SubmitChunksCommand
                {
                    UserId = userId,
                    FileItemId = fileItemId,
                    ChunksCount = chunksCount,
                    ChunksStorageSetting = chunksStorageSetting,
                    ApplicationId = applicationId
                };

                var fileItemDto = await _mediator.Send(submitChunksCommand, cancellationToken).ConfigureAwait(false);
                return Ok(fileItemDto);
            }
            catch (OperationCanceledException)
            {
                return BadRequest(ErrorCode.EC800);
            }
        }

        [HttpDelete("{fileItemId}")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteChunks")]
        public async Task<IActionResult> DeleteChunks(Guid fileItemId, Guid applicationId)
        {
            await _uploadedChunkService.DeleteAsync(fileItemId, applicationId).ConfigureAwait(false);

            _logger.Information($"File item '{fileItemId}' chunks was deleted.");

            return Ok(new OkDto());
        }
    }
}