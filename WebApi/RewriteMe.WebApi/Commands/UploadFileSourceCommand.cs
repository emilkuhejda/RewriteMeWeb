using System;
using Microsoft.AspNetCore.Http;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class UploadFileSourceCommand : CommandBase<FileItemDto>
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public string FileName { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid ApplicationId { get; set; }

        public IFormFile File { get; set; }
    }
}
