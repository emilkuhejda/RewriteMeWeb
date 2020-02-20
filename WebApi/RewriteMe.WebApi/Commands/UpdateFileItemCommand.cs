using System;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class UpdateFileItemCommand : CommandBase<FileItemDto>
    {
        public Guid FileItemId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
