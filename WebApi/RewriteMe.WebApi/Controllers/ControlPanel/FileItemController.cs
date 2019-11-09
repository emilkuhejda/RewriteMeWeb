using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class FileItemController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly IApplicationLogService _applicationLogService;

        public FileItemController(
            IFileItemService fileItemService,
            IApplicationLogService applicationLogService)
        {
            _fileItemService = fileItemService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("/api/control-panel/files/{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            try
            {
                var fileItems = await _fileItemService.GetAllForUserAsync(userId).ConfigureAwait(false);

                return Ok(fileItems);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/files/detail/{fileItemId}")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            try
            {
                var fileItem = await _fileItemService.GetAsAdminAsync(fileItemId).ConfigureAwait(false);

                return Ok(fileItem);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("/api/control-panel/files/restore")]
        public async Task<IActionResult> Restore(Guid userId, Guid fileItemId, Guid applicationId)
        {
            try
            {
                await _fileItemService.RestoreAllAsync(userId, new[] { fileItemId }, applicationId).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("/api/control-panel/files/update-recognition-state")]
        public async Task<IActionResult> UpdateRecognitionState(UpdateRecognitionStateModel updateModel)
        {
            try
            {
                var fileItemExists = await _fileItemService.ExistsAsync(updateModel.FileItemId, updateModel.FileName).ConfigureAwait(false);
                if (!fileItemExists)
                    return BadRequest();

                await _fileItemService.UpdateRecognitionStateAsync(updateModel.FileItemId, updateModel.RecognitionState, updateModel.ApplicationId).ConfigureAwait(false);
                var fileItem = await _fileItemService.GetAsAdminAsync(updateModel.FileItemId).ConfigureAwait(false);

                return Ok(fileItem);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}