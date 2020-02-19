using System;
using Microsoft.AspNetCore.Http;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Business.Commands
{
    public class UploadChunkFileCommand : CommandBase<OkDto>
    {
        public Guid FileItemId { get; set; }

        public int Order { get; set; }

        public StorageSetting StorageSetting { get; set; }

        public Guid ApplicationId { get; set; }

        public IFormFile File { get; set; }
    }
}
