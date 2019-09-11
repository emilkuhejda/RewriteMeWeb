﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public FileItemController(IFileItemService fileItemService)
        {
            _fileItemService = fileItemService;
        }

        [HttpGet("/api/control-panel/files/{userId}")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            var fileItems = await _fileItemService.GetAllForUserAsync(userId).ConfigureAwait(false);

            return Ok(fileItems);
        }

        [HttpGet("/api/control-panel/files/detail/{fileItemId}")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var fileItem = await _fileItemService.GetAsAdminAsync(fileItemId).ConfigureAwait(false);

            return Ok(fileItem);
        }

        [HttpPut("/api/control-panel/files/restore")]
        public async Task<IActionResult> Restore(Guid userId, Guid fileItemId, Guid applicationId)
        {
            await _fileItemService.RestoreAllAsync(userId, new[] { fileItemId }, applicationId).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("/api/control-panel/files/update-recognition-state")]
        public async Task<IActionResult> UpdateRecognitionState(UpdateRecognitionStateModel updateModel)
        {
            var fileItemExists = await _fileItemService.ExistsAsync(updateModel.FileItemId, updateModel.FileName).ConfigureAwait(false);
            if (!fileItemExists)
                return BadRequest();

            await _fileItemService.UpdateRecognitionStateAsync(updateModel.FileItemId, updateModel.RecognitionState, updateModel.ApplicationId).ConfigureAwait(false);
            var fileItem = await _fileItemService.GetAsAdminAsync(updateModel.FileItemId).ConfigureAwait(false);

            return Ok(fileItem);
        }
    }
}